using System;
using Morent.Application.Extensions;
using Morent.Application.Features.Rental.DTOs;
using Morent.Application.Features.Review.DTOs;
using Morent.Application.Interfaces;
using Morent.Application.Repositories;
using Morent.Core.ValueObjects;

namespace Morent.Infrastructure.Services;

public class UserService : IUserService, IUserProfileService
{
  private readonly IUserRepository _userRepository;
  private readonly IImageService _imageService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReviewRepository _reviewRepository;
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;

  public UserService(
      IUserRepository userRepository,
      IImageService imageService,
      IUnitOfWork unitOfWork,
      IReviewRepository reviewRepository,
      IRentalRepository rentalRepository,
      ICarRepository carRepository
      )
  {
    _userRepository = userRepository;
    _imageService = imageService;
    _unitOfWork = unitOfWork;
    _reviewRepository = reviewRepository;
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
  }

  public async Task<Result<UserProfileImageDto>> GetUserProfileImageAsync(Guid userId)
  {
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null || !user.ProfileImageId.HasValue)
      return Result.NotFound($"User with ID {userId} not found or doesn't have an profile image");

    var result = await _imageService.GetImageByIdAsync(user.ProfileImageId.Value);
    if (!result.IsSuccess)
      return Result.Error(new ErrorList(result.Errors));

    var imageResult = result.Value;

    return Result.Success(new UserProfileImageDto
    {
      ImageId = imageResult.Id,
      Url = imageResult.Url,
      UploadedAt = imageResult.UploadedAt
    });
  }

  public async Task<Result<UserProfileImageDto>> UpdateUserProfileImageAsync(Guid userId, ImageUploadRequest imageUpload)
  {
    if (imageUpload.ImageData.Length > 2 * 1024 * 1024) // 2MB limit for profile images
      return Result.Invalid(new ValidationError("Image", "Profile image exceeds maximum size of 2MB"));

    await _unitOfWork.BeginTransactionAsync();

    // Get user
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.NotFound($"User with ID {userId} not found");
    }

    // Store old image ID to delete later if exists
    var oldImageId = user.ProfileImageId;

    // Upload new image
    imageUpload.FileName = $"{user.Username}.{imageUpload.FileName.Split(".")[1]}";
    var uploadResult = await _imageService.UploadImageAsync(imageUpload);
    if (!uploadResult.IsSuccess)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.Error($"Failed to upload image: {string.Join(", ", uploadResult.Errors)}");
    }

    var uploadResponse = uploadResult.Value;

    // Update user profile image
    user.SetPrimaryImage(uploadResult.Value.ImageId);
    await _userRepository.UpdateAsync(user);
    await _unitOfWork.CommitTransactionAsync();

    // Delete old image in background if exists
    if (oldImageId.HasValue)
    {
      // Note: In a real system, this would be handled by a background job
      // to avoid transaction issues if the delete fails
      _ = Task.Run(async () =>
      {
        await _imageService.DeleteImageAsync(oldImageId.Value);
      });
    }

    // Return result
    return Result.Success(new UserProfileImageDto
    {
      ImageId = uploadResponse.ImageId,
      Url = uploadResponse.Url,
      UploadedAt = DateTime.UtcNow
    });
  }

  public async Task<Result> RemoveUserProfileImageAsync(Guid userId)
  {
    await _unitOfWork.BeginTransactionAsync();

    // Get user
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null || !user.ProfileImageId.HasValue)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.NotFound($"User with ID {userId} not found or doesn't have an profile image");
    }

    // Store image ID to delete
    var imageId = user.ProfileImageId.Value;

    // Remove profile image reference
    user.RemoveProfileImage();
    await _userRepository.UpdateAsync(user);

    // Delete the image
    await _imageService.DeleteImageAsync(imageId);

    await _unitOfWork.CommitTransactionAsync();
    return Result.Success();
  }

  public async Task<Result<UserProfileImageDto>> UpdateUserProfileImageAsync(Guid userId, string imageUrl)
  {
    Result<ImageUploadRequest> result = await _imageService.FetchImageAsUploadRequest(imageUrl);
    if (result.IsError())
      return Result.Error(new ErrorList(result.Errors));

    if (result.IsInvalid())
      return Result.Invalid(result.ValidationErrors);
    
    return await UpdateUserProfileImageAsync(userId, result.Value);
  }

  public async Task<Result<ReviewDto>> LeaveReviewAsync(Guid userId, Guid rentalId, int rating, string comment, CancellationToken cancellationToken = default)
  {
    // Verify the user has completed a rental for this car
    var rental = await _rentalRepository.GetByIdAsync(rentalId, cancellationToken);
    if (rental == null)
      return Result.NotFound($"Rental with ID {rentalId} not found");

    if (!rental.CanBeReviewedBy(userId))
      return Result.Forbidden("You can only review cars from completed rentals");

    // Check if user already reviewed this car
    if (await _reviewRepository.ExistsByUserAndCarAsync(userId, rental.CarId, cancellationToken))
      return Result.Conflict("You have already reviewed this car");

    // Create and save review
    var result = MorentReview.Create(userId, rental.CarId, rating, comment);
    if (!result.IsSuccess)
      return Result.Invalid(result.ValidationErrors);

    var review = result.Value;

    await _reviewRepository.AddAsync(review, cancellationToken);

    // Add review to car
    var car = await _carRepository.GetCarWithReviewsAsync(rental.CarId, cancellationToken);
    if (car == null)
      return Result.NotFound($"Car with ID {rental.CarId} not found");

    car.AddReview(review);
    await _carRepository.UpdateAsync(car, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    string? username = (await _userRepository.GetByIdAsync(userId))?.Username;
    if (username == null)
      return Result.CriticalError($"Cannot found user with ID {userId}.");

    var imageResult = await GetUserProfileImageAsync(userId);

    return Result.Created(new ReviewDto {
      Id = review.Id,
      UserId = review.UserId,
      UserImageUrl = imageResult.IsSuccess ? imageResult.Value.Url : "", 
      CarId = review.CarId,
      UserName = username,
      Rating = review.Rating,
      Comment = review.Comment,
      CreatedAt = review.CreatedAt
    });
  }

  public async Task<Result<RentalDto>> CreateRentalAsync(
      Guid userId,
      Guid carId,
      CreateRentalRequest request,
      CancellationToken cancellationToken = default)
  {
    // Validate car availability
    var car = await _carRepository.GetByIdAsync(carId, cancellationToken);
    if (car == null)
      return Result.NotFound($"Car with ID {carId} not found.");

    if (!car.IsAvailable)
      return Result.Error("Car is not available for rental");

    // Create rental period
    var rentalPeriod = DateRange.Create(request.PickupDate, request.DropoffDate);
    if (rentalPeriod.IsInvalid())
      return Result.Invalid(rentalPeriod.ValidationErrors);

    // Check if the car is already booked for the requested period
    var existingRentals = await _carRepository.GetActiveRentalsForCarAsync(carId, cancellationToken);
    foreach (var r in existingRentals)
    {
      if (r.RentalPeriod.Overlaps(rentalPeriod))
        return Result.Error("Car is already booked for the requested period");
    }

    // Create pickup and dropoff locations
    var pickupLocation = Location.Create(
      request.PickupLocation.Address, request.PickupLocation.City, request.PickupLocation.Country,
      request.PickupLocation.Longitude, request.PickupLocation.Latitude
      );
    if (pickupLocation.IsInvalid())
      return Result.Invalid(pickupLocation.ValidationErrors);

    var dropoffLocation = Location.Create(
      request.DropoffLocation.Address, request.DropoffLocation.City, request.DropoffLocation.Country,
      request.DropoffLocation.Longitude, request.DropoffLocation.Latitude
      );
    if (dropoffLocation.IsInvalid())
      return Result.Invalid(dropoffLocation.ValidationErrors);

    // Calculate total cost
    var totalDays = (int)Math.Ceiling((request.DropoffDate - request.PickupDate).TotalDays);
    if (totalDays < 1) totalDays = 1;

    var totalCost = car.PricePerDay.Multiply(totalDays);
    if (totalCost.IsInvalid()) 
      return Result.Invalid(totalCost.ValidationErrors);

    // Create rental
    var rentalId = Guid.NewGuid();
    var rentalResult = MorentRental.Create(
        rentalId,
        userId,
        carId,
        rentalPeriod,
        pickupLocation.Value,
        dropoffLocation.Value,
        totalCost);

    if (rentalResult.IsInvalid())
      return Result.Invalid(rentalResult.ValidationErrors);

    var rental = rentalResult.Value;

    // Save rental
    await _rentalRepository.AddAsync(rental, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Result.Success(rental.ToDto());
  }

  public async Task<Result> ActivateRentalAsync(
      Guid userId,
      Guid rentalId,
      CancellationToken cancellationToken = default)
  {
    var rental = await _rentalRepository.GetByIdAsync(rentalId, cancellationToken);
    if (rental == null)
      return Result.Error("Rental not found");

    if (rental.UserId != userId)
      return Result.Error("You are not authorized to activate this rental");

    // Start the rental
    var result = rental.StartRental();
    if (!result.IsSuccess)
      return Result.Error(new ErrorList(result.Errors));

    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return Result.Success();
  }

  public async Task<Result> CancelRentalAsync(
      Guid userId,
      Guid rentalId,
      CancellationToken cancellationToken = default)
  {
    var rental = await _rentalRepository.GetByIdAsync(rentalId, cancellationToken);
    if (rental == null)
      return Result.Error("Rental not found");

    if (rental.UserId != userId)
      return Result.Error("You are not authorized to cancel this rental");

    if (!rental.CanCancel())
      return Result.Error("Rental cannot be cancelled at this stage");

    // Cancel the rental
    var result = rental.CancelRental();
    if (!result.IsSuccess)
      return Result.Error(new ErrorList(result.Errors));

    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return Result.Success();
  }

  public async Task<Result> CompleteRentalAsync(
      Guid userId,
      Guid rentalId,
      CancellationToken cancellationToken = default)
  {
    var rental = await _rentalRepository.GetByIdAsync(rentalId, cancellationToken);
    if (rental == null)
      return Result.Error("Rental not found");

    if (rental.UserId != userId)
      return Result.Error("You are not authorized to complete this rental");

    if (!rental.IsActive())
      return Result.Error("Only active rentals can be completed");

    // Complete the rental
    var result = rental.CompleteRental();
    if (!result.IsSuccess)
      return Result.Error(new ErrorList(result.Errors));

    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return Result.Success();
  }
}
