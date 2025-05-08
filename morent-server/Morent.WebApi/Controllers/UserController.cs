using System.Security.Claims;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.User.Commands;
using Morent.Application.Interfaces;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserProfileService _userProfileImageService;
    private readonly IMediator _mediator;

    public UserController(IUserProfileService userProfileImageService, IMediator mediator)
    {
        _userProfileImageService = userProfileImageService;
        _mediator = mediator;
    }

    [HttpGet("{userId}/profile-image")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileImageDto>> GetUserProfileImage(Guid userId)
    {
        var result = await _userProfileImageService.GetUserProfileImageAsync(userId);
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

        var result = await _userProfileImageService.RemoveUserProfileImageAsync(userId);
        return this.ToActionResult(result);
    }

    private bool IsUserAuthorized(Guid userId)
    {
        // Check if the current user is either:
        // 1. The owner of the profile
        // 2. An admin
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        return isAdmin || (currentUserId != null && currentUserId == userId.ToString());
    }
}