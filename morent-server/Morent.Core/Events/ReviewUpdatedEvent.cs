using System;

namespace Morent.Core.Events;

public class ReviewUpdatedEvent : DomainEventBase
{
  public Guid ReviewId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }
  public int Rating { get; }

  public ReviewUpdatedEvent(Guid reviewId, Guid carId, Guid userId, int rating)
  {
    ReviewId = reviewId;
    CarId = carId;
    UserId = userId;
    Rating = rating;
  }
}
