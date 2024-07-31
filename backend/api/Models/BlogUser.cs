using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class BlogUser : IdentityUser
    {
        public string ProfilePicture { get; set; } = "/profiles/pictures/avatar-placeholder.png";
        public string Bio { get; set; } = String.Empty;
    }
}