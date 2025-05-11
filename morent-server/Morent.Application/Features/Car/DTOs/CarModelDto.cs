using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarModelDto
{
  public required string Brand { get; set; } = default!;
  public required string Model { get; set; } = default!;
  public required string FuelType { get; set; } = default!;
  public required string GearBox { get; set; } = default!;
  public required int FuelTankCapacity { get; set; }
  public required int Year { get; set; }
  public required int SeatCapacity { get; set; }
  public required string Type { get; set; } = default!;
}