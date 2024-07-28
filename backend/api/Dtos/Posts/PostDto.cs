using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Models;

namespace api.Dtos.Posts
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserDto Author { get; set; } = new UserDto();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        
        public List<string> Tags { get; set; } = new List<string>();
    }
}