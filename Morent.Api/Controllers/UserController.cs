using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morent.Api.Helpers;
using Morent.Core.Entities;
using Morent.Core.Interfaces;

namespace Morent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMorentDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(IMorentDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var users = await _context.Users.AnyAsync(u => u.Id == id);
            return Ok(users);
        }

        [Authorize]
        [HttpPost("{userId}/profile-image")]
        public async Task<IActionResult> UploadProfileImage(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found.");

            try
            {
                var (fileName, _) = await FileHelper.SaveImageFileAsync(file);

                var image = new MorentImage
                {
                    FileName = fileName,
                    Url = FileHelper.GetFileUrl(fileName),
                    UserId = userId
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();
                return Ok(image);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for user {userId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
