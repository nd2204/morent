using System;
using Morent.Core.MorentRentalAggregate;

namespace Morent.Core.MorentCarAggregate;

public class MorentOwnedCar : EntityBase
{
  public int CarId { get; set; }
  public string? Vin { get; set; }
  public MorentOwnedCarStatus Status { get; set; } = MorentOwnedCarStatus.Available;
  public string? Description { get; set; }
  private readonly List<MorentRental> _rentals = new();
  public IEnumerable<MorentRental> Rentals => _rentals.AsReadOnly();

  public void AddRental(MorentRental rental)
  {
    Guard.Against.Null(rental);
    _rentals.Add(rental);
  }
}
