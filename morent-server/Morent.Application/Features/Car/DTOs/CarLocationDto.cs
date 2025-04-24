using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarLocationDto
{
  public string City { get; set; } = default!;
  public string Address { get; set; } = default!;
  public string Country { get; set; } = default!;
}
