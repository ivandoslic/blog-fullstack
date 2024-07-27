using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Posts;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDbContext _context;

        public PostRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post?> DeleteAsync(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return null;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post?> UpdateAsync(int id, UpdatePostDto stockDto)
        {
            var currentPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (currentPost == null)
            {
                return null;
            }

            currentPost.Title = stockDto.Title;
            currentPost.Content = stockDto.Content;
            currentPost.IsDraft = stockDto.IsDraft;
            // currentPost.Tags = stockDto.Tags;
            currentPost.UpdatedAt = DateTime.Now.ToUniversalTime();

            await _context.SaveChangesAsync();

            return currentPost;
        }
    }
}