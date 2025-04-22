using System;

namespace Morent.Application.Features.Review;

public class LeaveReviewCommandHandler : ICommandHandler<LeaveReviewCommand, Guid>
{
  private readonly IReviewRepository _reviewRepository;
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;
  private readonly ICurrentUserService _currentUserService;
  // private readonly IUnitOfWork _unitOfWork;

  public LeaveReviewCommandHandler(
      IReviewRepository reviewRepository,
      IRentalRepository rentalRepository,
      ICarRepository carRepository,
      // IUnitOfWork unitOfWork,
      ICurrentUserService currentUserService)
  {
    _reviewRepository = reviewRepository;
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
    _currentUserService = currentUserService;
    // _unitOfWork = unitOfWork;
  }

  public async Task<Guid> Handle(LeaveReviewCommand command, CancellationToken cancellationToken)
  {
    if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
      throw new UnauthorizedAccessException("User must be authenticated to leave a review");

    var userId = _currentUserService.UserId.Value;

    // Verify the user has completed a rental for this car
    var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    if (rental == null)
      throw new ApplicationException($"Rental with ID {command.RentalId} not found");

    if (!rental.CanBeReviewedBy(userId))
      throw new ApplicationException("You can only review cars from completed rentals");

    // Check if user already reviewed this car
    if (await _reviewRepository.ExistsByUserAndCarAsync(userId, command.CarId, cancellationToken))
      throw new ApplicationException("You have already reviewed this car");

    // Create and save review
    var review = MorentReview.Create(userId, command.CarId, command.Rating, command.Comment);

    await _reviewRepository.AddAsync(review, cancellationToken);

    // Add review to car
    var car = await _carRepository.GetCarWithReviewsAsync(command.CarId, cancellationToken);
    car.AddReview(review);
    await _carRepository.UpdateAsync(car, cancellationToken);

    // Add domain event
    // var domainEvent = new ReviewAddedEvent(reviewId, command.CarId, userId, command.Rating);

    // await _unitOfWork.SaveChangesAsync(cancellationToken);
    await _carRepository.SaveChangesAsync(cancellationToken);

    return review.Value.Id;
  }
}