namespace Morent.Core.Events;

public class RentalCompletedEvent : DomainEventBase
{
  public Guid RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalCompletedEvent(Guid rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}