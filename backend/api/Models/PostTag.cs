using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("PostTag")]
    public class PostTag
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}