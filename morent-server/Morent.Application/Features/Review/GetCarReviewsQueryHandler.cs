using System;

namespace Morent.Application.Features.Review;

public class GetCarReviewsQueryHandler : IQueryHandler<GetCarReviewsQuery, PagedResult<IEnumerable<ReviewDto>>>
{
  private readonly IReviewRepository _reviewRepository;
  private readonly IUserRepository _userRepository;
  private readonly IUserProfileService _userProfileService;

  public GetCarReviewsQueryHandler(
      IReviewRepository reviewRepository,
      IUserProfileService userProfileService,
      IUserRepository userRepository)
  {
    _reviewRepository = reviewRepository;
    _userRepository = userRepository;
    _userProfileService = userProfileService;
  }

  public async Task<PagedResult<IEnumerable<ReviewDto>>> Handle(GetCarReviewsQuery query, CancellationToken cancellationToken)
  {
    var reviews = await _reviewRepository.GetReviewsByCarIdAsync(query.CarId, cancellationToken);

    var reviewDtos = new List<ReviewDto>();
    foreach (var review in reviews)
    {
      var user = await _userRepository.GetByIdAsync(review.UserId, cancellationToken);
      var result = await _userProfileService.GetUserProfileImageAsync(review.UserId);

      if (!result.IsSuccess) continue;

      var userImage = result.Value;

      reviewDtos.Add(new ReviewDto
      {
        Id = review.Id,
        UserId = review.UserId,
        UserName = user?.Name ?? "Anonymous",
        CarId = review.CarId,
        ImageUrl = userImage.Url,
        CarDetails = string.Empty, // Could be populated if needed
        Rating = review.Rating,
        Comment = review.Comment,
        CreatedAt = review.CreatedAt
      });
    }

    // For now just returns maximum 10 reviews
    int totalRecords = reviews.Count();
    int pageSize = query.PagedQuery.PageSize;
    int page = query.PagedQuery.Page;

    return new PagedResult<IEnumerable<ReviewDto>>(
      new PagedInfo(page, pageSize, totalRecords / pageSize, totalRecords),
      reviewDtos
    );
  }
}