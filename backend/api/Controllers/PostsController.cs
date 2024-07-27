using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Posts;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepo;

        public PostsController(IPostRepository postRepo)
        {
            _postRepo = postRepo;
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

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto postData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = postData.ToPostFromCreateDto();

            await _postRepo.CreateAsync(post);

            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post.ToPostDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePostDto updatePostData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await _postRepo.DeleteAsync(id);

            if (post == null) 
            {
                return NotFound();
            }

            return Ok(post.ToPostDto()); // Return this DTO to user in case he'd do something with the deleted posts info
        }
    }
}