using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.Car.Queries;

namespace Morent.Api.Controllers.Car;

[Route("api/cars")]
[ApiController]
public class CarController : ControllerBase
{
  private readonly ILogger<CarController> _logger;
  private readonly IMediator _mediator;

  public CarController(
    IMediator mediator,
    ILogger<CarController> logger)
  {
    _logger = logger;
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<CarDto>>> GetCars([FromQuery] GetCarsQuery query)
  {
    var result = await _mediator.Send(new GetCarsByQuery(query));

    var pagedInfo = result.PagedInfo;
    Response.Headers.Append("X-Total-Count", pagedInfo.TotalRecords.ToString());
    Response.Headers.Append("X-Page-Number", pagedInfo.PageNumber.ToString());
    Response.Headers.Append("X-Page-Size", pagedInfo.PageSize.ToString());
    Response.Headers.Append("X-Total-Pages", pagedInfo.TotalPages.ToString());

    return this.ToActionResult(result);
  }

  [HttpGet("near")]
  public async Task<ActionResult<IEnumerable<CarLocationDto>>> GetNearCarsLocation([FromQuery] GetNearCarsLocationQuery request)
  {
    var result = await _mediator.Send(request);
    return this.ToActionResult(result);
 }

  [HttpGet("{carId:guid}")]
  public async Task<ActionResult<CarDetailDto>> GetCarById(Guid carId)
  {
    var result = await _mediator.Send(new GetCarByIdQuery(carId));
    return this.ToActionResult(result);
  }

  // =============================================================================
  // Car Reviews 
  // =============================================================================

  [HttpGet("{carId:guid}/reviews")]
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

  // =============================================================================
  // Car Images 
  // =============================================================================
  [HttpGet("{carId:Guid}/images")]
  [AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IEnumerable<CarImageDto>>> GetCarImages(Guid carId)
  {
    var imagesResult = await _mediator.Send(new GetCarImagesQuery(carId));
    return this.ToActionResult(imagesResult);
  }
}