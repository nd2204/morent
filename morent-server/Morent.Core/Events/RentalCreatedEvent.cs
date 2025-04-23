namespace Morent.Core.Events;

public class RentalCreatedEvent : DomainEventBase
{
  public int RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }
  public DateRange RentalPeriod { get; }

  public RentalCreatedEvent(int rentalId, Guid carId, Guid userId, DateRange rentalPeriod)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
    RentalPeriod = rentalPeriod;
  }
}