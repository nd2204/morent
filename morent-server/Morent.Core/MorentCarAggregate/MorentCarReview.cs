namespace Morent.Core.MorentUserAggregate;

public class MorentCarReview : EntityBase
{
  public MorentCarReview(Guid userId, int carId, decimal rating, string comment)
  {
    UserId = userId;
    CarId = carId;
    Rating = rating;
    Comment = comment;
    CreatedAt = DateTime.UtcNow;
  }

  public Guid UserId        { get; private set; }
  public int CarId          { get; private set; }
  public decimal Rating     { get; private set; }
  public string Comment     { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; }

  private readonly List<MorentCarReview> _reviewResponses = new();
  public IEnumerable<MorentCarReview> ReviewResponsesList => _reviewResponses.AsReadOnly();

  public void AddResponse(MorentCarReview response)
  {
    Guard.Against.Null(response, nameof(response));
    _reviewResponses.Add(response);

    // TODO: Add domain event for review response added
    // RegisterDomainEvent(new MorentReviewResponseAddedEvent(this, response));
  }
}
