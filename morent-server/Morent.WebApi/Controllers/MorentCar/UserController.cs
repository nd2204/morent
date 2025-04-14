// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Morent.Api.Helpers;
// using Morent.Infrastructure.Data;

// namespace Morent.Api.Controllers
// {
//     [Authorize]
//     [Route("api/[controller]")]
//     [ApiController]
//     public class UserController : ControllerBase
//     {
//         private readonly MorentDbContext _context;
//         private readonly ILogger<UserController> _logger;

//         public UserController(MorentDbContext context, ILogger<UserController> logger)
//         {
//             _context = context;
//             _logger = logger;
//         }

//         [HttpGet("{id:guid}")]
//         public async Task<IActionResult> GetUser(Guid id)
//         {
//             var users = await _context.Users.AnyAsync(u => u.Id == id);
//             return Ok(users);
//         }

//         [HttpPost("{userId:guid}/profile-image")]
//         public async Task<IActionResult> UploadProfileImage(Guid userId, IFormFile file)
//         {
//             if (file == null || file.Length == 0)
//                 return BadRequest("No file uploaded.");

//             var user = await _context.Users.FindAsync(userId);
//             if (user == null) return NotFound("User not found.");

//             try
//             {
//                 var (fileName, _) = await FileHelper.SaveImageFileAsync(file);

//                 var image = new MorentImage
//                 {
//                     FileName = fileName,
//                     Url = FileHelper.GetFileUrl(fileName),
//                     UserId = userId
//                 };

//                 _context.Images.Add(image);
//                 await _context.SaveChangesAsync();
//                 return Ok(image);
//             } catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error uploading image for user {userId}", userId);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//     }
// }
