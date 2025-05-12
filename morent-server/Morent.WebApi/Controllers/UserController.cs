using System.Security.Claims;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.User.Commands;
using Morent.Application.Features.User.Queries;
using Morent.Application.Interfaces;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    private readonly IMediator _mediator;

    public UserController(IUserProfileService userProfileImageService, IMediator mediator)
    {
        _userProfileService = userProfileImageService;
        _mediator = mediator;
    }

    // Public endpoints ==========================================================

    [HttpGet("{userId}/profile-image")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileImageDto>> GetUserProfileImage(Guid userId)
    {
        var result = await _userProfileService.GetUserProfileImageAsync(userId);
        return this.ToActionResult(result);
    }

    [HttpPost("{userId}/profile-image")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserProfileImageDto>> UploadUserProfileImage(
        Guid userId,
        IFormFile image)
    {
        // Check if user is allowed to update this profile image
        if (!IsUserAuthorized(userId))
            return Forbid();

        if (image == null || image.Length == 0)
            return BadRequest("No image file provided");

        var result = await _mediator.Send(new UploadUserProfileImageCommand(userId, image));
        return this.ToActionResult(result);
    }

    [HttpDelete("{userId}/profile-image")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteUserProfileImage(Guid userId)
    {
        // Check if user is allowed to delete this profile image
        if (!IsUserAuthorized(userId))
        {
            return Forbid();
        }

        var result = await _userProfileService.RemoveUserProfileImageAsync(userId);
        return this.ToActionResult(result);
    }

    // User private endpoints ====================================================

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetAuthorizedUserInfo()
    {
        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserByIdQuery(userId));
        return this.ToActionResult(result);
    }

    [HttpGet("me/profile-image")]
    [Authorize]
    public async Task<ActionResult<UserProfileImageDto>> GetAuthorizedUserProfileImage()
    {
        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserProfileImageQuery(userId));
        return this.ToActionResult(result);
    }

    [HttpGet("me/rentals")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetUserRentalsInfo()
    {
        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserRentalsQuery(userId, null));
        return this.ToActionResult(result);
    }

    [HttpGet("me/reviews")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserCarsReviewDto>>> GetUserReviews()
    {
        if (!User.Identity!.IsAuthenticated)
            return Unauthorized();

        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserReviewsQuery(userId));
        return this.ToActionResult(result);
    }


    // Private Methods ============================================================

    private bool IsUserAuthorized(Guid userId)
    {
        // Check if the current user is either:
        // 1. The owner of the profile
        // 2. An admin
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        return isAdmin || (currentUserId != null && currentUserId == userId.ToString());
    }

    private Guid GetUserGuid()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
    }
}