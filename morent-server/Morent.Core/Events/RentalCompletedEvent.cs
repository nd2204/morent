namespace Morent.Core.Events;

public class RentalCompletedEvent : DomainEventBase
{
  public int RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalCompletedEvent(int rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}