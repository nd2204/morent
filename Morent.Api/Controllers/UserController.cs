using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetUser(int id)
        {
            var users = await _context.Users.AnyAsync(u => u.Id == id);
            return Ok(users);
        }

        [Authorize]
        [HttpPost("{userId}/profile-image")]
        public async Task<IActionResult> UploadProfileImage(int userId, IFormFile file)
        {
            return BadRequest("Not implemented yet.");
        }
    }
}
