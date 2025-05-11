using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarLocationDto
{
  public required string City { get; set; } = default!;
  public required string Address { get; set; } = default!;
  public required string Country { get; set; } = default!;
}
