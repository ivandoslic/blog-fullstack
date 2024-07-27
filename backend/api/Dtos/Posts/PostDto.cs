using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Posts
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        
        // TODO: Add when user management and authentication is handled:
        // public string AuthorId = String.Empty;
        
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}