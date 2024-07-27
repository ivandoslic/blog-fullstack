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
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string BlogUserId { get; set; } = String.Empty;
        public BlogUser BlogUser { get; set; }
        
        public int? PostId { get; set; }
        public Post? Post { get; set; }
        
    }
}