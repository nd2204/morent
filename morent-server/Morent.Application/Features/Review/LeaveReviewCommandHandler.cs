using System;

namespace Morent.Application.Features.Review;

public class LeaveReviewCommandHandler : ICommandHandler<LeaveReviewCommand, Result<Guid>>
{
  private readonly IReviewRepository _reviewRepository;
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;
  private readonly IUnitOfWork _unitOfWork;

  public LeaveReviewCommandHandler(
      IReviewRepository reviewRepository,
      IRentalRepository rentalRepository,
      IUnitOfWork unitOfWork,
      ICarRepository carRepository)
  {
    _reviewRepository = reviewRepository;
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task<Result<Guid>> Handle(LeaveReviewCommand command, CancellationToken cancellationToken)
  {
    // Verify the user has completed a rental for this car
    // var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    // if (rental == null)
    //   return Result.NotFound($"Rental with ID {command.RentalId} not found");

    // if (!rental.CanBeReviewedBy(command.UserId))
    //   return Result.Forbidden("You can only review cars from completed rentals");

    // Check if user already reviewed this car
    if (await _reviewRepository.ExistsByUserAndCarAsync(command.UserId, command.CarId, cancellationToken))
      return Result.Conflict("You have already reviewed this car");

    // Create and save review
    var review = MorentReview.Create(command.UserId, command.CarId, command.Rating, command.Comment);
    await _reviewRepository.AddAsync(review, cancellationToken);

    // Add review to car
    var car = await _carRepository.GetCarWithReviewsAsync(command.CarId, cancellationToken);
    if (car == null)
      return Result.NotFound($"Car with ID {command.CarId} not found");

    car.AddReview(review);
    await _carRepository.UpdateAsync(car, cancellationToken);

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Result.Created(review.Value.Id);
  }
}