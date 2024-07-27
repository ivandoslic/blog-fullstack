using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Posts")]
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime UpdatedAt { get; set; }
        public bool IsDraft { get; set; } = true;

        public string BlogUserId { get; set; }
        public BlogUser BlogUser { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}