using System;

namespace Morent.Application.Features.Car;

public class CreateCarCommand : ICommand<Guid>
{
  public string Brand { get; set; } = null!;
  public string Model { get; set; } = null!;
  public int Year { get; set; }
  public string LicensePlate { get; set; } = null!;
  public FuelType FuelType { get; set; }
  public decimal PricePerDay { get; set; }
  public string Currency { get; set; } = null!;
  public int Capacity { get; set; }
  public LocationDto Location { get; set; } = null!;
  public List<string> Images { get; set; } = null!;
}