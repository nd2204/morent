using System;

namespace Morent.Core.MorentCarAggregate;

public class MorentCarModel : EntityBase<Guid>
{
  public string Brand { get; private set; }
  public string ModelName { get; private set; }
  public int Year { get; private set; }
  public FuelType FuelType { get; private set; }
  public GearBoxType Gearbox { get; private set; }
  public int FuelTankCapacity { get; private set; }
  public int SeatCapacity { get; private set; }
  public CarType CarType { get; private set; }

  private MorentCarModel() { }

  public MorentCarModel(Guid id, string brand, string modelName, int year, FuelType fuelType, GearBoxType gearbox, CarType carType, int fuelTankCapacity, int seatCapacity)
  {
    Guard.Against.NullOrEmpty(brand, nameof(brand));
    Guard.Against.NullOrEmpty(modelName, nameof(modelName));

    Id = id;
    Brand = brand;
    ModelName = modelName;
    Year = year;
    FuelType = fuelType;
    Gearbox = gearbox;
    FuelTankCapacity = fuelTankCapacity;
    SeatCapacity = seatCapacity;
    CarType = carType;
  }


  // public static Result<MorentCarModel> Create(Guid id, string brand, string modelName, int year, FuelType fuelType, GearBoxType gearbox, int seatCapacity)
  // {
  //   if (string.IsNullOrWhiteSpace(brand))
  //     return Result.Invalid(new ValidationError("Car", "Brand cannot be empty."));

  //   if (string.IsNullOrWhiteSpace(modelName))
  //     return Result.Invalid(new ValidationError("Car", "Model cannot be empty."));

  //   if (year< 1900 || year> DateTime.UtcNow.Year + 1)
  //     return Result.Invalid(new ValidationError("Car", $"Year must be between 1900 and {DateTime.UtcNow.Year + 1}."));

  //   if (seatCapacity <= 0)
  //     return Result.Invalid(new ValidationError("Car", "Capacity must be greater than zero."));

  //   return Result.Success(new MorentCarModel(id, brand, modelName, year, fuelType, gearbox, seatCapacity));
  // }
}
