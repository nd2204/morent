using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarDetailDto : CarDto
{
  public string Description { get; set; } = default!;
  public List<ReviewDto> Reviews { get; set; } = new();
  public CarLocationDto Location { get; set; } = default!;
}
