using Morent.Core.Exceptions;

namespace Morent.Core.Entities;

public class MorentReview : EntityBase<Guid>, IAggregateRoot
{
  public Guid UserId { get; private set; }
  public Guid CarId { get; private set; }
  public int Rating { get; private set; }
  public string Comment { get; private set; }
  public DateTime CreatedAt { get; private set; }

  private MorentReview(Guid userId, Guid carId, int rating, string comment)
  {
    Id = Guid.NewGuid();
    UserId = userId;
    CarId = carId;
    Rating = rating;
    Comment = comment;
    CreatedAt = DateTime.UtcNow;
  }

  public static Result<MorentReview> Create(Guid userId, Guid carId, int rating, string comment)
  {
    if (userId == Guid.Empty)
      return Result.Invalid(new ValidationError("Review", "User ID cannot be empty."));

    if (carId == Guid.Empty)
      return Result.Invalid(new ValidationError("Review", "Car ID cannot be empty."));

    if (rating < 1 || rating > 5)
      return Result.Invalid(new ValidationError("Review", "Rating must be between 1 and 5."));

    var review = new MorentReview(userId, carId, rating, comment);
    review.RegisterDomainEvent(new ReviewCreatedEvent(review.Id, carId, userId, rating));

    return Result.Success(review);
  }

  public Result UpdateReview(int rating, string comment)
  {
    if (rating < 1 || rating > 5)
      return Result.Invalid(new ValidationError("Review", "Rating must be between 1 and 5."));

    Rating = rating;
    Comment = comment;
    this.RegisterDomainEvent(new ReviewUpdatedEvent(Id, CarId, UserId, rating));

    return Result.Success();
  }

  public void UpdateRating(int rating)
  {
    Guard.Against.InvalidInput(rating, nameof(rating), r => rating >= 1 || rating <= 5);
    Rating = rating;
  }

  public void UpdateComment(string comment)
  {
    Comment = comment;
  }
}