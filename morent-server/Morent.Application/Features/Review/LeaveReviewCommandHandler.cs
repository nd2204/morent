using System;

namespace Morent.Application.Features.Review;

public class LeaveReviewCommandHandler : ICommandHandler<LeaveReviewCommand, Guid>
{
  private readonly IReviewRepository _reviewRepository;
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;
  // private readonly IUnitOfWork _unitOfWork;

  public LeaveReviewCommandHandler(
      IReviewRepository reviewRepository,
      IRentalRepository rentalRepository,
      // IUnitOfWork unitOfWork,
      ICarRepository carRepository)
  {
    _reviewRepository = reviewRepository;
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
    // _unitOfWork = unitOfWork;
  }

  public async Task<Guid> Handle(LeaveReviewCommand command, CancellationToken cancellationToken)
  {
    // Verify the user has completed a rental for this car
    var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    if (rental == null)
      throw new ApplicationException($"Rental with ID {command.RentalId} not found");

    if (!rental.CanBeReviewedBy(command.UserId))
      throw new ApplicationException("You can only review cars from completed rentals");

    // Check if user already reviewed this car
    if (await _reviewRepository.ExistsByUserAndCarAsync(command.UserId, command.CarId, cancellationToken))
      throw new ApplicationException("You have already reviewed this car");

    // Create and save review
    var review = MorentReview.Create(command.UserId, command.CarId, command.Rating, command.Comment);

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