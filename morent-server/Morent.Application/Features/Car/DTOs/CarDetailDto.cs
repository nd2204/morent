using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarDetailDto : CarDto
{
  public required string Description { get; set; } = default!;
  public required List<ReviewDto> Reviews { get; set; } = new();
  public required LocationDto Location { get; set; } = default!;
}
