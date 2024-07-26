using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedAt = DateTime.Now;

        // TODO: Add when user management and authentication is handled:
        // public string AuthorId = String.Empty;
        
        public int? PostId { get; set; }
        public Post? Post { get; set; }
        
    }
}