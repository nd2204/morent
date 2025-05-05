using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Features.Review;

namespace Morent.WebApi.Controllers;

[Route("api/reviews")]
[ApiController]
public class CarReviewController : ControllerBase
{
    private readonly IMediator _mediator;
    public CarReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("car/{id:guid}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetCarReviews(Guid id)
    {
        var result = await _mediator.Send(new GetCarReviewsQuery(id));

        var pagedInfo = result.PagedInfo;
        Response.Headers.Append("X-Total-Count", pagedInfo.TotalRecords.ToString());
        Response.Headers.Append("X-Page-Number", pagedInfo.PageNumber.ToString());
        Response.Headers.Append("X-Page-Size", pagedInfo.PageSize.ToString());
        Response.Headers.Append("X-Total-Pages", pagedInfo.TotalPages.ToString());

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPost("car/{id:guid}")]
    public async Task<ActionResult<Guid>> LeaveReview(Guid id, [FromBody] LeaveReviewCommand request)
    {
        var result = await _mediator.Send(request);
        return this.ToActionResult(result);
    }
}