namespace Morent.Core.Events;

public class RentalCancelledEvent : DomainEventBase
{
  public Guid RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalCancelledEvent(Guid rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}
