using System;

namespace Morent.Application.Features.Review;

public class GetCarReviewsQueryHandler : IQueryHandler<GetCarReviewsQuery, IEnumerable<ReviewDto>>
{
  private readonly IReviewRepository _reviewRepository;
  private readonly IUserRepository _userRepository;

  public GetCarReviewsQueryHandler(
      IReviewRepository reviewRepository,
      IUserRepository userRepository)
  {
    _reviewRepository = reviewRepository;
    _userRepository = userRepository;
  }

  public async Task<IEnumerable<ReviewDto>> Handle(GetCarReviewsQuery query, CancellationToken cancellationToken)
  {
    var reviews = await _reviewRepository.GetReviewsByCarIdAsync(query.CarId, cancellationToken);

    var reviewDtos = new List<ReviewDto>();
    foreach (var review in reviews)
    {
      var user = await _userRepository.GetByIdAsync(review.UserId, cancellationToken);

      reviewDtos.Add(new ReviewDto
      {
        Id = review.Id,
        UserId = review.UserId,
        UserName = user?.Name ?? "Anonymous",
        CarId = review.CarId,
        CarDetails = string.Empty, // Could be populated if needed
        Rating = review.Rating,
        Comment = review.Comment,
        CreatedAt = review.CreatedAt
      });
    }

    return reviewDtos;
  }
}