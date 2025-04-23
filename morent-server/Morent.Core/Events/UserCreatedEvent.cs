namespace Morent.Core.Events;

public class UserCreatedEvent : DomainEventBase
{
  public Guid UserId { get; }

  public UserCreatedEvent(Guid userId)
  {
    UserId = userId;
  }
}
