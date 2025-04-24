namespace Morent.Core.Events;

public class RentalConfirmedEvent : DomainEventBase
{
  public Guid RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalConfirmedEvent(Guid rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}