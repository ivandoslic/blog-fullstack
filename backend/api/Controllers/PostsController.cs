using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Posts;
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
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepo;
        private readonly UserManager<BlogUser> _userManager;

        public PostsController(IPostRepository postRepo, UserManager<BlogUser> userManager)
        {
            _postRepo = postRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var posts = await _postRepo.GetAllAsync();

            var postDto = posts.Select(p => p.ToPostDto()).ToList();

            return Ok(postDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await _postRepo.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post.ToPostDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreatePostDto postData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = User.GetUserName();
            var blogUser = await _userManager.FindByNameAsync(userName);

            if (blogUser == null)
            {
                return StatusCode(500, "Could not fetch user data");
            }

            var post = postData.ToPostFromCreateDto();

            post.BlogUserId = blogUser.Id;

            try {
                var postResult = await _postRepo.CreateAsync(post, postData.Tags);

                if (postResult == null)
                {
                    return StatusCode(500, "Could not create the post");
                }

                return CreatedAtAction(nameof(GetById), new { id = post.Id }, post.ToPostDto());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePostDto updatePostData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = User.GetUserName();
            var blogUser = await _userManager.FindByNameAsync(userName);

            if (blogUser == null)
            {
                return StatusCode(500, "Could not fetch user data");
            }

            var belongs = await _postRepo.BelongsToAsync(id, blogUser.Id);

            if (belongs == null)
            {
                return StatusCode(500);
            }

            if (belongs == false)
            {
                return Unauthorized("You can't modify post that is not yours");
            }

            var post = await _postRepo.UpdateAsync(id, updatePostData);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = User.GetUserName();
            var blogUser = await _userManager.FindByNameAsync(userName);

            if (blogUser == null)
            {
                return StatusCode(500, "Could not fetch user data");
            }

            var belongs = await _postRepo.BelongsToAsync(id, blogUser.Id);

            if (belongs == null)
            {
                return StatusCode(500);
            }

            if (belongs == false)
            {
                return Unauthorized("You can't modify post that is not yours");
            }

            var post = await _postRepo.DeleteAsync(id);

            if (post == null) 
            {
                return NotFound();
            }

            return Ok(post.ToPostDto()); // Return this DTO to user in case he'd do something with the deleted posts info
        }

        // Tags:

        [HttpPut("{postId:int}/tags")]
        [Authorize]
        public async Task<IActionResult> AddTagsToPost([FromRoute] int postId, [FromBody] List<string> tags)
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

            var belong = await _postRepo.BelongsToAsync(postId, blogUser.Id);

            if (belong == null)
            {
                return StatusCode(500);
            }

            if (belong == false)
            {
                return Unauthorized("You can't add tags to post that is not yours");
            }

            try {
                await _postRepo.AddTagsToPostAsync(postId, tags);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{postId:int}/tags")]
        public async Task<IActionResult> RemoveTagsFromPost(int postId, List<string> tags)
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

            var belong = await _postRepo.BelongsToAsync(postId, blogUser.Id);

            if (belong == null)
            {
                return StatusCode(500);
            }

            if (belong == false)
            {
                return Unauthorized("You can't add tags to post that is not yours");
            }

            try {
                await _postRepo.RemoveTagsFromPostAsync(postId, tags);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}