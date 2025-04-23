namespace Morent.Core.Events;

public class RentalConfirmedEvent : DomainEventBase
{
  public int RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }

  public RentalConfirmedEvent(int rentalId, Guid carId, Guid userId)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
  }
}