using System;
using Morent.Application.Extensions;

namespace Morent.Application.Features.Review;

public class GetUserReviewQueryHandler(
  IReviewRepository reviewRepository,
  IRentalRepository rentalRepository,
  IUserRepository userRepository,
  ICarImageService carImageService,
  ICarRepository carRepository,
  IImageService imageService
) : IQueryHandler<GetUserReviewsQuery, Result<IEnumerable<UserCarsReviewDto>>>
{
  private readonly IReviewRepository _reviewRepository = reviewRepository;
  private readonly ICarRepository _carRepository = carRepository;
  private readonly IRentalRepository _rentalRepository = rentalRepository;
  private readonly IUserRepository _userRepository = userRepository;
  private readonly ICarImageService _carImageService = carImageService;
  private readonly IImageService _imageService = imageService;

  public async Task<Result<IEnumerable<UserCarsReviewDto>>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByIdAsync(request.UserId);
    if (user == null)
      return Result.NotFound($"User with ID {request.UserId} not found.");

    var rentals = await _rentalRepository.GetRentalsByUserIdAsync(request.UserId);

    var reviewDtos = new List<UserCarsReviewDto>();
    foreach (var rental in rentals)
    {
      var car = await _carRepository.GetCarWithRentalsAsync(rental.CarId);
      if (car == null)
        return Result.NotFound($"Car with ID {rental.CarId} notfound");

      var reviews = car.Reviews;
      
      var imageResult = await _carImageService.GetCarImagesAsync(rental.CarId);

      var review = reviews.FirstOrDefault(s => s.UserId == request.UserId);
      bool isReviewed = review != null;

      var carDto = car.ToCarDto();
      carDto.Images.ForEach(
        async image => image = await SetCarImageUrl(image));

      reviewDtos.Add(new UserCarsReviewDto
      {
        Rental = rental.ToDto(),
        Car = carDto,
        CarImageUrl = imageResult.IsSuccess ? imageResult.Value.First().Url : "",
        Rating = isReviewed ? review!.Rating : 0,
        Comment = isReviewed ? review!.Comment : "",
        IsReviewed = isReviewed
      });
    }

    return Result.Success(reviewDtos.AsEnumerable());
  }
  private async Task<CarImageDto> SetCarImageUrl(CarImageDto carImage)
  {
    var result = await _imageService.GetImageByIdAsync(carImage.ImageId);
    if (!result.IsSuccess)
    {
      var placeholderImageResult = await _imageService.GetPlaceHolderImageAsync();
      carImage.Url = placeholderImageResult.Value.Url;
      return carImage;
    }
    carImage.Url = result.Value.Url;
    return carImage;
  }
}

