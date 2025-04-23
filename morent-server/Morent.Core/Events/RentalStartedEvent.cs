namespace Morent.Core.Events;

public class RentalStartedEvent : DomainEventBase
{
  public int RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalStartedEvent(int rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}