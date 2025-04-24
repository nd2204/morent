using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarModelDto
{
  public string Brand { get; set; } = default!;
  public string Model { get; set; } = default!;
  public string FuelType { get; set; } = default!;
  public string GearBox { get; set; } = default!;
  public int FuelTankCapacity { get; set; }
  public int Year { get; set; }
  public int SeatCapacity { get; set; }
}