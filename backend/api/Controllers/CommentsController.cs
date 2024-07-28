using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly UserManager<BlogUser> _userManager;

        public CommentsController(ICommentRepository commentRepo, UserManager<BlogUser> userManager)
        {
            _commentRepo = commentRepo;
            _userManager = userManager;
        }

        [HttpGet("{postId:int}")]
        public async Task<IActionResult> GetPostComments([FromRoute] int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepo.GetAllPostCommentsAsync(postId);      

            if (comments == null || comments.Count == 0)
            {
                return NotFound("No comments for this post");
            }

            return Ok(comments.Select(c => c.ToCommentDto()));
        }

        [HttpPost("{postId:int}")]
        [Authorize]
        public async Task<IActionResult> PostComment([FromRoute] int postId, [FromBody] string commentContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.GetUserName();
            var blogUser = await _userManager.FindByNameAsync(username);

            if (blogUser == null)
            {
                return StatusCode(500, "Could not fetch user data");
            }

            var commentDto = new CreateCommentDto
            {
                PostId = postId,
                BlogUserId = blogUser.Id,
                Content = commentContent
            };

            var result = await _commentRepo.CreateCommentAsync(commentDto);

            if (result == null)
            {
                return StatusCode(500, "Can't post comment right now");
            }

            return Ok(result.ToCommentDto());
        }

        [HttpDelete("{commentId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteAComment([FromRoute] int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var username = User.GetUserName();
           var user = await _userManager.FindByNameAsync(username);

           if (user == null)
           {
                return StatusCode(500, "Could not fetch user data");
           }

           var belongs = await _commentRepo.BelongsToAsync(commentId, user.Id);

           if (belongs == null)
           {
                return StatusCode(500);
           }

           if (belongs == false)
           {
                return Unauthorized("You can only delete your comments");
           }

           var result = await _commentRepo.DeleteCommentAsync(commentId);

            if (result == null)
            {
                return NotFound();
            }

            Console.WriteLine(result);

            return NoContent();
        }
    }
}