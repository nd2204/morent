namespace Morent.Core.Events;

public class RentalCreatedEvent : DomainEventBase
{
  public Guid RentalId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }
  public DateRange RentalPeriod { get; }

  public RentalCreatedEvent(Guid rentalId, Guid carId, Guid userId, DateRange rentalPeriod)
  {
    RentalId = rentalId;
    CarId = carId;
    UserId = userId;
    RentalPeriod = rentalPeriod;
  }
}