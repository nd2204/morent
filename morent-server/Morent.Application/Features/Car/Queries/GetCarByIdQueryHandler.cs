using System;
using Morent.Application.Extensions;
using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Car;

public class GetCarByIdQueryHandler : IQueryHandler<GetCarByIdQuery, Result<CarDetailDto>>
{
  private readonly ICarRepository _carRepository;
  private readonly IReviewRepository _reviewRepository;
  private readonly IUserProfileService _userProfileService;
  private readonly IImageService _imageService;

  public GetCarByIdQueryHandler(
      ICarRepository carRepository,
      IReviewRepository reviewRepository,
      IUserProfileService userProfileService,
      IImageService imageService)
  {
    _carRepository = carRepository;
    _reviewRepository = reviewRepository;
    _userProfileService = userProfileService;
    _imageService = imageService;
  }

  public async Task<Result<CarDetailDto>> Handle(GetCarByIdQuery query, CancellationToken cancellationToken)
  {
    var car = await _carRepository.GetCarWithReviewsAsync(query.Id, cancellationToken);

    if (car == null)
      return Result.NotFound($"Car with ID {query.Id} not found");

    var carDetail = car.ToCarDetailDto();
    foreach (var image in carDetail.Images)
    {
      var imageResult = await _imageService.GetImageByIdAsync(image.ImageId);
      if (imageResult.IsSuccess)
        image.Url = imageResult.Value.Url;
    }

    foreach (var review in carDetail.Reviews)
    {
      var imageResult = await _userProfileService.GetUserProfileImageAsync(review.UserId);
      if (imageResult.IsSuccess)
        review.UserImageUrl = imageResult.Value.Url;
    }

    return Result.Success(carDetail);
  }
}