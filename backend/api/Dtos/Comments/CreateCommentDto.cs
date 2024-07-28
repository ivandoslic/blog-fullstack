using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class CreateCommentDto
    {
        public required int PostId { get; set; }
        public required string Content { get; set; }
        public required string BlogUserId { get; set; }
    }
}