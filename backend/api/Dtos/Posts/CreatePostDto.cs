using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Posts
{
    public class CreatePostDto
    {
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        [MaxLength(5, ErrorMessage = "A post cannot have more than 5 tags")]
        public List<string> Tags = new List<string>();
    }
}