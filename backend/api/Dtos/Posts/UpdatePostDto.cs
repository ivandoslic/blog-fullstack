using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Posts
{
    public class UpdatePostDto
    {
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public bool IsDraft { get; set; } = true;
        // public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}