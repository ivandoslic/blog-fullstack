using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    public class ProfilesController : ControllerBase
    {
        private readonly UserManager<BlogUser> _userManager;

        public ProfilesController(UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("pictures")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return StatusCode(500, "Could not fetch user data");
            }

            try
            {
                var fileName = $"{user.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                var filePath = Path.Combine("wwwroot", "profiles", "pictures", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/profiles/pictures/{fileName}";

                user.ProfilePicture = fileUrl;

                await _userManager.UpdateAsync(user);

                return Ok(new { fileUrl });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}