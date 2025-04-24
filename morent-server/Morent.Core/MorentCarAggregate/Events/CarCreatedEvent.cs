namespace Morent.Core.Events;

public class CarCreatedEvent : DomainEventBase
{
  public Guid CarId { get; }

  public CarCreatedEvent(Guid carId)
  {
    CarId = carId;
  }
}
