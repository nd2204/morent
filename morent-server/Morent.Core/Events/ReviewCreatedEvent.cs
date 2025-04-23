using System;

namespace Morent.Core.Events;

public class ReviewCreatedEvent : DomainEventBase
{
  public int ReviewId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }
  public int Rating { get; }

  public ReviewCreatedEvent(int reviewId, Guid carId, Guid userId, int rating)
  {
    ReviewId = reviewId;
    CarId = carId;
    UserId = userId;
    Rating = rating;
  }
}