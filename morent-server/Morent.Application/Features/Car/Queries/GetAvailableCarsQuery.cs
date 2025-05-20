using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Car;

public class GetAvailableCarsQuery : IQuery<IEnumerable<CarDto>>
{
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public LocationDto NearLocation { get; set; } = null!;
  public int? MinCapacity { get; set; }
  public string Brand { get; set; } = null!;
  public FuelType? FuelType { get; set; }
}