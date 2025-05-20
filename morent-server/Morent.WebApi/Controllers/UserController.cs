using System.Security.Claims;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.Rental;
using Morent.Application.Interfaces;

[ApiController]
[Authorize]
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

    // User private endpoints ====================================================

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetAuthorizedUserInfo()
    {
        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserByIdQuery(userId));
        return this.ToActionResult(result);
    }

    [HttpGet("me/profile-image")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileImageDto>> GetAuthorizedUserProfileImage()
    {
        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserProfileImageQuery(userId));
        return this.ToActionResult(result);
    }

    [HttpPost("me/profile-image")]
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

    [HttpDelete("me/profile-image")]
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

    [HttpGet("me/rentals")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetUserRentalsInfo()
    {
        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserRentalsQuery(userId, null));
        return this.ToActionResult(result);
    }

    [HttpPost("me/rentals")]
    public async Task<ActionResult<RentalDto>> CreateRental([FromBody] CreateRentalRequest request)
    {
        var userId = GetUserGuid();
        var result = await _mediator.Send(new RentCarCommand
        {
            UserId = userId,
            CarId = request.CarId,
            PickupDate = request.PickupDate,
            DropoffDate = request.DropoffDate,
            PickupLocation = request.PickupLocation,
            DropoffLocation = request.DropoffLocation,
        });
        return this.ToActionResult(result);
    }

    [HttpGet("me/reviews")]
    public async Task<ActionResult<IEnumerable<UserCarsReviewDto>>> GetUserReviews()
    {
        if (!User.Identity!.IsAuthenticated)
            return Unauthorized();

        var userId = GetUserGuid();
        var result = await _mediator.Send(new GetUserReviewsQuery(userId));
        return this.ToActionResult(result);
    }

    [HttpPost("me/reviews")]
    public async Task<ActionResult<ReviewDto>> LeaveReview([FromBody] LeaveReviewRequest request)
    {
        var result = await _mediator.Send(new LeaveReviewCommand
        {
            UserId = GetUserGuid(),
            Request = request
        });
        return this.ToActionResult(result);
    }

    [HttpPut("me/reviews/{reviewId:guid}")]
    public async Task<ActionResult<Guid>> UpdateReview(Guid reviewId, [FromBody] UpdateReviewRequest request)
    {
        var result = await _mediator.Send(new UpdateReviewCommand
        {
            UserId = GetUserGuid(),
            ReviewId = reviewId,
            Rating = request.Rating,
            Comment = request.Comment
        });
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