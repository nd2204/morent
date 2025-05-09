using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Repositories;
using Morent.Core.MorentUserAggregate;

namespace Morent.WebApi.Controllers;

[Route("api/rentals")]
[ApiController]
public class Rental : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRentalRepository _rentalRepository;

    public Rental(IMediator mediator, IRentalRepository rentalRepository)
    {
        _mediator = mediator;
        _rentalRepository = rentalRepository;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetAllRental()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<RentalDto>> GetRentalById(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("car/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> CreateRental(Guid carId, [FromBody] CreateRentalRequest request)
    {
        throw new NotImplementedException();
    }
}
