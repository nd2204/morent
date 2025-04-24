using System;

namespace Morent.Application.Features.Review;

public class GetCarReviewsQuery : IQuery<IEnumerable<ReviewDto>>
{
  public Guid CarId { get; set; }
}
