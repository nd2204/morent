namespace Morent.Application.Features.Car.DTOs;

  public class CarDto
  {
    public Guid Id { get; set; }
    public CarModelDto CarModel { get; set; } = default!;
    public string LicensePlate { get; set; } = default!;
    public decimal PricePerDay { get; set; }
    public string Currency { get; set; } = default!;
    public List<CarImageDto> Images { get; set; } = default!;
    public bool IsAvailable { get; set; }
    public double AverageRating { get; set; }
    public int ReviewsCount { get; set; }
  }
