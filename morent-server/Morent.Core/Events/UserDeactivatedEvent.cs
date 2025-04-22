namespace Morent.Core.Events;

public class UserDeactivatedEvent : DomainEventBase
{
  public Guid UserId { get; }

  public UserDeactivatedEvent(Guid userId)
  {
    UserId = userId;
  }
}
