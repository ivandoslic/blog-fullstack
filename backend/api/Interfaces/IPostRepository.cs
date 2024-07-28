using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Posts;
using api.Models;

namespace api.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> CreateAsync(Post post, List<string> tags);
        Task<Post?> UpdateAsync(int id, UpdatePostDto stockDto);
        Task<Post?> DeleteAsync(int id);
        Task<bool?> BelongsToAsync(int postId, string userId);
        Task AddTagsToPostAsync(int postId, List<string> tags);
        Task RemoveTagsFromPostAsync(int postId, List<string> tags);
    }
}