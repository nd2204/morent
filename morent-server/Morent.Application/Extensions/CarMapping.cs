namespace Morent.Application.Extensions;

public static class CarMapping
{
  public static CarDetailDto ToCarDetailDto(this MorentCar car)
  {
    var reviewDtos = new List<ReviewDto>();
    foreach (var review in car.Reviews)
    {
      reviewDtos.Add(review.ToDto());
    }

    double averageRating = reviewDtos.Any()
        ? reviewDtos.Average(r => r.Rating)
        : 0;

    string carTitle = $"{car.CarModel.Brand} {car.CarModel.ModelName} {car.CarModel.Year}";

    return new CarDetailDto
    {
      Id = car.Id,
      Title = carTitle,
      Description = car.Description ?? "N/a",
      LicensePlate = car.LicensePlate,
      PricePerDay = car.PricePerDay.Amount,
      Currency = car.PricePerDay.Currency,
      IsAvailable = car.IsAvailable,
      AverageRating = averageRating,
      ReviewsCount = reviewDtos.Count,
      Images = car.Images.ToDtoList(),
      CarModel = car.CarModel.ToDto(),
      Reviews = reviewDtos,
      Location = car.CurrentLocation.ToDto()
    };
  }

  public static CarDto ToCarDto(this MorentCar car)
  {
    var reviews = car.Reviews.ToList();
    double averageRating = reviews.Any()
        ? reviews.Average(r => r.Rating)
        : 0;

    string carTitle = $"{car.CarModel.Brand} {car.CarModel.ModelName} {car.CarModel.Year}";

    return new CarDto
    {
      Id = car.Id,
      Title = carTitle,
      LicensePlate = car.LicensePlate,
      PricePerDay = car.PricePerDay.Amount,
      Currency = car.PricePerDay.Currency,
      IsAvailable = car.IsAvailable,
      AverageRating = averageRating,
      ReviewsCount = reviews.Count,
      Images = car.Images.ToDtoList(),
      CarModel = car.CarModel.ToDto(),
    };

  }

  public static CarModelDto ToDto(this MorentCarModel model)
  {
    return new CarModelDto
    {
      Brand = model.Brand,
      Model = model.ModelName,
      Year = model.Year,
      SeatCapacity = model.SeatCapacity,
      FuelType = model.FuelType.ToString(),
      GearBox = model.Gearbox.ToString(),
      FuelTankCapacity = model.FuelTankCapacity,
      Type = model.CarType.ToString()
    };
  }

  public static ReviewDto ToDto(this MorentReview model)
  {
    return new ReviewDto
    {
      Id = model.Id,
      CarId = model.CarId,
      UserImageUrl = "",
      UserName = model.User.Name,
      UserId = model.UserId,
      Comment = model.Comment,
      Rating = model.Rating,
      CreatedAt = model.CreatedAt
    };
  }

  public static LocationDto ToDto(this Location location)
  {
    return new LocationDto
    {
      Address = location.Address!,
      City = location.City!,
      Country = location.Country!,
      Longitude = location.Longitude,
      Latitude = location.Latitude,
    };
  }

  public static CarLocationDto ToCarLocationDto(this MorentCar car)
  {
    return new CarLocationDto
    {
      CarId = car.Id,
      Title = GetCarTitle(car),
      CarModel = car.CarModel.ToDto(),
      ImageUrl = "",
      Longitude = car.CurrentLocation.Longitude,
      Latitude = car.CurrentLocation.Latitude,
    };
  }

  public static Location ToEntity(this LocationDto location)
  {
    var result = Location.Create(
      address: location.Address,
      city: location.City,
      country: location.Country,
      longitude: location.Longitude,
      latitude: location.Latitude
    );

    if (!result.IsSuccess)
      throw new Exception("Invalid location");

    return result;
  }

  public static CarImageDto ToDto(this MorentCarImage image)
  {
    return new CarImageDto {
      ImageId = image.ImageId,
      Url = "",
      IsPrimary = image.IsPrimary,
      DisplayOrder = image.DisplayOrder
    };
  }

  public static List<CarImageDto> ToDtoList(this IEnumerable<MorentCarImage> images)
  {
    var carImageDtos = new List<CarImageDto>();
    foreach (var image in images)
    {
      carImageDtos.Add(image.ToDto());
    }
    return carImageDtos;
  }

  private static string GetCarTitle(MorentCar car)
  {
    return $"{car.CarModel.Brand} {car.CarModel.ModelName} {car.CarModel.Year}";
  }
}