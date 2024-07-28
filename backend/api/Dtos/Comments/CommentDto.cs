using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
    }
}