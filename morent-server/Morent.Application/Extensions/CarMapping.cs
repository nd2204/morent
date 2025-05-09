using System;
using Morent.Application.Features.Car.DTOs;

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

    return new CarDetailDto
    {
      Id = car.Id,
      LicensePlate = car.LicensePlate,
      PricePerDay = car.PricePerDay.Amount,
      Currency = car.PricePerDay.Currency,
      IsAvailable = car.IsAvailable,
      AverageRating = averageRating,
      ReviewsCount = reviewDtos.Count,
      Images = car.Images.ToDtoList(),
      CarModel = car.CarModel.ToDto(),
      Reviews = reviewDtos,
      Location = new CarLocationDto
      {
        Address = car.CurrentLocation.Address,
        City = car.CurrentLocation.City,
      }
    };

  }

  public static CarDto ToCarDto(this MorentCar car)
  {
    var reviews = car.Reviews.ToList();
    double averageRating = reviews.Any()
        ? reviews.Average(r => r.Rating)
        : 0;

    return new CarDto
    {
      Id = car.Id,
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
      GearBox = model.Gearbox.ToString()
    };
  }

  public static ReviewDto ToDto(this MorentReview model)
  {
    return new ReviewDto
    {
      UserId = model.UserId,
      Comment = model.Comment,
      Rating = model.Rating,
      CreatedAt = model.CreatedAt
    };
  }

  public static CarLocationDto ToDto(this Location location)
  {
    return new CarLocationDto
    {
      Address = location.Address,
      City = location.City,
      Country = location.Country
    };
  }

  public static Location ToEntity(this CarLocationDto location)
  {
    return Location.Create(
      address: location.Address,
      city: location.City,
      country: location.Country
    ).Value;
  }

  public static CarImageDto ToDto(this MorentCarImage image)
  {
    return new CarImageDto {
      ImageId = image.Id,
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
}