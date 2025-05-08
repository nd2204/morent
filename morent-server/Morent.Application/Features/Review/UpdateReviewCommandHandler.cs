using System;

namespace Morent.Application.Features.Review;

public class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, Result>
{
  private readonly IReviewRepository _reviewRepository;
  private readonly IUserRepository _userRepository;

  public UpdateReviewCommandHandler(IReviewRepository reviews, IUserRepository userRepository)
  {
    _reviewRepository = reviews;
    _userRepository = userRepository;
  }

  public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
  {
    var review = await _reviewRepository.GetByIdAsync(request.ReviewId);
    if (review == null)
      return Result.NotFound($"Review with ID ${request.ReviewId} not found.");

    // Find user with the request userId
    var user = await _userRepository.GetByIdAsync(request.UserId);
    if (user == null)
      return Result.NotFound($"User with ID ${request.UserId} not found.");

    // INFO: This should be a suspicious activity since client should
    //       not allow this behavior
    if (user.Id != review.UserId)
    {
      return Result.Forbidden("Access denied. You are not allowed to access this resource.");
    }

    review.UpdateReview(request.Rating, request.Comment);
    var count = await _reviewRepository.UpdateAsync(review);

    if (count < 0) 
      return Result.CriticalError("Unexpected error during update");

    return Result.NoContent();
  }
}
