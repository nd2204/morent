using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.Car;
using Morent.Application.Features.Car.Commands;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Features.Car.Queries;
using Morent.Infrastructure.Data;

namespace Morent.Api.Controllers;

[Route("api/cars")]
[ApiController]
public class CarsController : ControllerBase
{
  private readonly MorentDbContext _context;
  private readonly ILogger<CarsController> _logger;
  private readonly IMediator _mediator;

  public CarsController(
    MorentDbContext context,
    IMediator mediator,
    ILogger<CarsController> logger)
  {
    _context = context;
    _logger = logger;
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult<PagedResult<CarDto>>> GetCars([FromQuery] GetCarsQuery query)
  {
    var result = await _mediator.Send(new GetCarsByQuery(query));
    return Ok(result);
  }

  [HttpGet("{id:guid}")]
  public async Task<ActionResult<CarDetailDto>> GetCarById(Guid id)
  {
    var car = await _mediator.Send(new GetCarByIdQuery(id));
    if (car == null) return NotFound("Không tìm thấy xe (với id cụ thể)");
    return Ok(car);
  }

  // Car CRUD ================================================================================
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
}