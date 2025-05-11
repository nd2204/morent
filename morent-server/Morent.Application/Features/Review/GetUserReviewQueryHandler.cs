using System;

namespace Morent.Application.Features.Review;

public class GetUserReviewQueryHandler(
  IReviewRepository reviewRepository,
  IRentalRepository rentalRepository,
  IUserRepository userRepository,
  ICarImageService carImageService
) : IQueryHandler<GetUserReviewsQuery, Result<IEnumerable<UserCarsReviewDto>>>
{
  private readonly IReviewRepository _reviewRepository = reviewRepository;
  private readonly IRentalRepository _rentalRepository = rentalRepository;
  private readonly IUserRepository _userRepository = userRepository;
  private readonly ICarImageService _carImageService = carImageService;

  public async Task<Result<IEnumerable<UserCarsReviewDto>>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByIdAsync(request.UserId);
    if (user == null)
      return Result.NotFound($"User with ID {request.UserId} not found.");

    var reviews = await _reviewRepository.GetReviewsByUserIdAsync(request.UserId);

    var reviewDtos = new List<UserCarsReviewDto>();
    foreach (var review in reviews)
    {
      var rental = await _rentalRepository.GetRentalByUserAndCarAsync(request.UserId, review.CarId);

      if (rental == null)
        return Result.CriticalError($"Rental by user with Id {request.UserId} and car ID {review.CarId} not found!");
      
      var result = await _carImageService.GetCarImagesAsync(review.CarId);

      var carReviews = await _reviewRepository.GetReviewsByCarIdAsync(review.CarId);

      reviewDtos.Add(new UserCarsReviewDto
      {
        RentalId = rental.Id,
        CarImageUrl = result.IsSuccess ? result.Value.First().Url : "",
        Rating = review.Rating,
        IsReviewed = carReviews.Any(s => s.UserId == request.UserId)
      });
    }

    return Result.Success(reviewDtos.AsEnumerable());
  }
}
