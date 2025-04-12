using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morent.Core.Helpers;
using Morent.Infrastructure.Data;

namespace Morent.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly MorentDbContext _context;

        public AuthController(MorentDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login()
        {
            var form = await Request.ReadFormAsync();
            var username = form["username-or-email"].ToString().Trim();
            var password = form["password"].ToString().Trim();

            var user = await _context.Users.FirstOrDefaultAsync(u => (u.Email == username || u.Username == username));

            if (user == null) return NotFound("User not found.");

            // if (Security.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            // {
            //     var token = Security.GenerateJwtToken(user.Username, user.Role);
            //     return Ok(new { token });
            // }

            return Unauthorized();
        }

        [HttpPost("signup")]
        public ActionResult Signup()
        {
            return BadRequest("Not Implemented yet.");
        }


        [HttpPost("logout")]
        public ActionResult Logout()
        {
            return BadRequest("Not Implemented yet.");
        }
    }
}
