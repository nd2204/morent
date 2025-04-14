using System.ComponentModel.DataAnnotations.Schema;
using Morent.Core.MorentCarAggregate;
using Morent.Core.MorentUserAggregate;

namespace Morent.Core.MorentRentalAggregate;

public class MorentRental : EntityBase<int>, IAggregateRoot
{
  public string RentalStatus { get; set; } = null!;

  public Guid UserId { get; set; }
  public MorentUser User { get; set; } = null!;

  public int OwnedCarId { get; set; }
  public MorentOwnedCar Car { get; set; } = null!;

  public int RentalDetailId { get; private set; }
  public MorentRentalDetail RentalDetail { get; set; } = null!;

  public MorentPayment? Payment { get; set; }
}
