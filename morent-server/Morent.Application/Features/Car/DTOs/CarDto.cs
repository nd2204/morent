namespace Morent.Application.Features.Car.DTOs;

  public class CarDto
  {
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required CarModelDto CarModel { get; set; } = default!;
    public required string LicensePlate { get; set; } = default!;
    public required decimal PricePerDay { get; set; }
    public required string Currency { get; set; } = default!;
    public required List<CarImageDto> Images { get; set; } = default!;
    public required bool IsAvailable { get; set; }
    public required double AverageRating { get; set; }
    public required int ReviewsCount { get; set; }
  }
