namespace Morent.Core.Events;

public class RentalCancelledEvent : DomainEventBase
{
  public int RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalCancelledEvent(int rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}
