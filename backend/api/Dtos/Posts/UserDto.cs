using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Posts
{
    public class UserDto
    {
        public string UserId { get; set; } = String.Empty;
        public string UserName { get; set; } = "unknown";
    }
}