using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Tags")]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}