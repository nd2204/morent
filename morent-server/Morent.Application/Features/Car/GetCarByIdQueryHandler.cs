using System;

namespace Morent.Application.Features.Car;

public class GetCarByIdQueryHandler : IQueryHandler<GetCarByIdQuery, CarDto>
{
  private readonly ICarRepository _carRepository;
  private readonly IReviewRepository _reviewRepository;

  public GetCarByIdQueryHandler(
      ICarRepository carRepository,
      IReviewRepository reviewRepository)
  {
    _carRepository = carRepository;
    _reviewRepository = reviewRepository;
  }

  public async Task<CarDto> Handle(GetCarByIdQuery query, CancellationToken cancellationToken)
  {
    var car = await _carRepository.GetCarWithReviewsAsync(query.Id, cancellationToken);
    if (car == null)
      throw new ApplicationException($"Car with ID {query.Id} not found");

    var reviews = car.Reviews.ToList();
    double? averageRating = reviews.Any()
        ? reviews.Average(r => r.Rating)
        : null;

    return new CarDto
    {
      Id = car.Id,
      Brand = car.Brand,
      Model = car.Model,
      Year = car.Year,
      LicensePlate = car.LicensePlate,
      FuelType = car.FuelType,
      PricePerDay = car.PricePerDay.Amount,
      Currency = car.PricePerDay.Currency,
      Capacity = car.Capacity,
      Images = car.Images.ToList(),
      IsAvailable = car.IsAvailable,
      CurrentLocation = new LocationDto
      {
        Address = car.CurrentLocation.Address,
        City = car.CurrentLocation.City,
        State = car.CurrentLocation.State,
        ZipCode = car.CurrentLocation.ZipCode,
        Country = car.CurrentLocation.Country,
        Latitude = car.CurrentLocation.Latitude,
        Longitude = car.CurrentLocation.Longitude
      },
      AverageRating = averageRating,
      ReviewsCount = reviews.Count
    };
  }
}