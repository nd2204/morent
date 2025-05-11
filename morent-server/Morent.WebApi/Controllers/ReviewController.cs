using System.Security.Claims;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Morent.WebApi.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("car/{carId:guid}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetCarReviews(Guid carId)
    {
        var result = await _mediator.Send(new GetCarReviewsQuery(carId));

        var pagedInfo = result.PagedInfo;
        Response.Headers.Append("X-Total-Count", pagedInfo.TotalRecords.ToString());
        Response.Headers.Append("X-Page-Number", pagedInfo.PageNumber.ToString());
        Response.Headers.Append("X-Page-Size", pagedInfo.PageSize.ToString());
        Response.Headers.Append("X-Total-Pages", pagedInfo.TotalPages.ToString());

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPost("car/{carId:guid}")]
    public async Task<ActionResult<ReviewDto>> LeaveReview(Guid carId, [FromBody] LeaveReviewRequest request)
    {
        var result = await _mediator.Send(new LeaveReviewCommand
        {
            UserId = GetUserIdFromAuth(),
            CarId = carId,
            Request = request
        });
        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPut("{reviewId:guid}")]
    public async Task<ActionResult<Guid>> UpdateReview(Guid reviewId, [FromBody] UpdateReviewRequest request)
    {
        var result = await _mediator.Send(new UpdateReviewCommand
        {
            UserId = GetUserIdFromAuth(),
            ReviewId = reviewId
        });
        return this.ToActionResult(result);
    }

    private Guid GetUserIdFromAuth()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
    }
}