using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comments;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;

        public CommentRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<bool?> BelongsToAsync(int commentId, string userId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return null;
            }

            return comment.BlogUserId == userId;
        }

        public async Task<Comment?> CreateCommentAsync(CreateCommentDto commentData)
        {
            var comment = commentData.CreateDtoToComment();
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(comment);
            Console.ForegroundColor = ConsoleColor.White;

            if (comment == null)
            {
                return null;
            }

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();

            return comment;
        }

        public Task<List<Comment>> GetAllPostCommentsAsync(int postId)
        {
            return _context.Comments.Include(c => c.BlogUser).Where(c => c.PostId == postId).ToListAsync();
        }
    }
}