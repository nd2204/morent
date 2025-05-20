namespace Morent.Application.Features.Car.DTOs;

public class CarLocationDto
{
  public required Guid CarId { get; set; }
  public required string Title { get; set; }
  public required string ImageUrl { get; set; } = default!;
  public required CarModelDto CarModel { get; set; } = default!;
  public required double Longitude { get; set; }
  public required double Latitude { get; set; }
}
