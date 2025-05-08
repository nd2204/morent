using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.Car;
using Morent.Application.Features.Car.Commands;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Features.Car.Queries;
using Morent.Infrastructure.Data;

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

  [HttpGet("{id:guid}")]
  public async Task<ActionResult<CarDetailDto>> GetCarById(Guid id)
  {
    var result = await _mediator.Send(new GetCarByIdQuery(id));
    return this.ToActionResult(result);
  }

  // Car CRUD ====================================================================
  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<IActionResult> CreateCar([FromBody] CreateCarCommand request)
  {
    var result = await _mediator.Send(request);
    return Ok(result);
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> UpdateCar(Guid id, [FromBody] UpdateCarCommand request)
  {
    var result = await _mediator.Send(new UpdateCarCommand());
    return Ok(result);
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> DeleteCar(Guid id)
  {
    var result = await _mediator.Send(new DeleteCarCommand(id));
    return Ok(result);
  }

  // Car Images ==================================================================
  [HttpGet("{carId:Guid}/images")]
  [AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IEnumerable<CarImageDto>>> GetCarImages(Guid carId)
  {
    var imagesResult = await _mediator.Send(new GetCarImagesQuery(carId));
    return this.ToActionResult(imagesResult);
  }

  [HttpPost("{carId:guid}/images")]
  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<CarImageDto>> UploadCarImage(
      Guid carId,
      [FromForm] UploadCarImageRequest request)
  {
    if (request.Image == null || request.Image.Length == 0)
    {
      return BadRequest("No image file provided");
    }

    var result = await _mediator.Send(
      new UploadCarImageCommand(
        carId, request.Image, request.ImageUrl, request.SetAsPrimary));
      
    return this.ToActionResult(result);
  }

  [HttpPost("{carId:guid}/images/{imageId:guid}")]
  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> DeleteCarImage(Guid carId, Guid imageId)
  {
    var result = await _mediator.Send(new DeleteCarImageCommand(carId, imageId));
    return this.ToActionResult(result);
  }

  [HttpPut("{carId:guid}/images/{imageId:guid}/set-primary")]
  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<CarImageDto>> SetPrimaryImage(Guid carId, Guid imageId)
  {
    var result = await _mediator.Send(new SetCarPrimaryImageCommand(carId, imageId));
    return this.ToActionResult(result);
  }

  [HttpPut("{carId:guid}/images/reorder")]
  [Authorize(Roles = "Admin,Manager")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> ReorderCarImages(
      Guid carId,
      [FromBody] List<CarImageOrderItem> newOrder)
  {
    var result = await _mediator.Send(new ReorderCarImagesCommand(carId, newOrder));
    return this.ToActionResult(result);
  }
}