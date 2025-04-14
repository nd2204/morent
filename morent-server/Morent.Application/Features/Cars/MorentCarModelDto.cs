namespace Morent.Application.Features.MorentCarModel;

public record class MorentCarModelDto(
  int Id,
  int Capacity,
  string Brand,
  string Model,
  int FuelCapacityLitter,
  string SteeringType,
  decimal PricePerDay,
  int CarTypeId
);