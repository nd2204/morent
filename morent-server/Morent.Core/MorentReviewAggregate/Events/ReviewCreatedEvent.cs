namespace Morent.Core.MorentReviewAggregate.Events;

public class ReviewCreatedEvent : DomainEventBase
{
  public Guid ReviewId { get; }
  public Guid CarId { get; }
  public Guid UserId { get; }
  public int Rating { get; }

  public ReviewCreatedEvent(Guid reviewId, Guid carId, Guid userId, int rating)
  {
    ReviewId = reviewId;
    CarId = carId;
    UserId = userId;
    Rating = rating;
  }
}