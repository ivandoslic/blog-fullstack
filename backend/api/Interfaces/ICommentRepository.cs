using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllPostCommentsAsync(int postId);
        public Task<Comment?> CreateCommentAsync(CreateCommentDto commenData);
        public Task<Comment?> DeleteCommentAsync(int commentId);
        Task<bool?> BelongsToAsync(int commentId, string userId);
    }
}