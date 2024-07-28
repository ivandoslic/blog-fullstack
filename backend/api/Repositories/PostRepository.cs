using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Posts;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<bool?> BelongsToAsync(int postId, string userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return null;
            }

            return post.BlogUserId == userId;
        }

        public async Task<Post> CreateAsync(Post post, List<string> tags)
        {
            if (tags.Count > 5) {
                throw new ArgumentException("A post cannot have more than 5 tags");
            }

            foreach (var tag in tags)
            {
                var dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tag.ToLower());

                if (dbTag == null)
                {
                    dbTag = new Tag { Name = tag.ToLower() };
                    _context.Tags.Add(dbTag);
                }

                post.PostTags.Add(new PostTag { Tag = dbTag });
            }

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
            return await _context.Posts
            .Include(u => u.BlogUser)
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.Comments)
            .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts
            .Include(u => u.BlogUser)
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task AddTagsToPostAsync(int postId, List<string> tags)
        {
            var post = await _context.Posts.Include(p => p.PostTags).FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                throw new ArgumentException("Post not found");
            }

            if (post.PostTags.Count + tags.Count > 5)
            {
                throw new ArgumentException("A post cannot have more than 5 tags");
            }

            foreach (var tag in tags)
            {
                var dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tag.ToLower());
                if (dbTag == null)
                {
                    dbTag = new Tag { Name = tag.ToLower() };
                    await _context.Tags.AddAsync(dbTag);
                }

                if (!post.PostTags.Any(pt => pt.TagId == dbTag.Id))
                {
                    post.PostTags.Add(new PostTag { Tag = dbTag });
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveTagsFromPostAsync(int postId, List<string> tags)
        {
            var post = await _context.Posts.Include(p => p.PostTags).FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new ArgumentException("Post not found");
            }

            foreach (var tag in tags)
            {
                var dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tag.ToLower());

                if (dbTag == null)
                {
                    throw new ArgumentException("There is no tag " + tag.ToLower() + " in database");
                }

                var postTag = post.PostTags.FirstOrDefault(pt => pt.TagId == dbTag.Id);

                if (postTag == null)
                {
                    throw new ArgumentException("There is no tag " + tag.ToLower() + " for this post");
                }
            }

            foreach (var tag in tags)
            {
                var dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tag.ToLower()) ?? throw new ArgumentException("Tag " + tag.ToLower() + " not found");
                var postTag = await _context.PostTags.FirstOrDefaultAsync(pt => pt.PostId == postId && pt.TagId == dbTag.Id);
                if (postTag != null)
                {
                    _context.PostTags.Remove(postTag);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}