namespace Morent.Core.Events;

public class RentalStartedEvent : DomainEventBase
{
  public Guid RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalStartedEvent(Guid rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}