using System;

namespace Morent.Application.Features.Car;

public class GetAvailableCarsQueryHandler : IQueryHandler<GetAvailableCarsQuery, IEnumerable<CarDto>>
{
  private readonly ICarRepository _carRepository;
  private readonly IReviewRepository _reviewRepository;

  public GetAvailableCarsQueryHandler(
      ICarRepository carRepository,
      IReviewRepository reviewRepository)
  {
    _carRepository = carRepository;
    _reviewRepository = reviewRepository;
  }

  public async Task<IEnumerable<CarDto>> Handle(GetAvailableCarsQuery query, CancellationToken cancellationToken)
  {
    var cars = await _carRepository.GetAvailableCarsAsync(
        query.StartDate,
        query.EndDate,
        query.NearLocation,
        query.MinCapacity,
        cancellationToken);

    // Filter by additional criteria if provided
    if (!string.IsNullOrWhiteSpace(query.Brand))
    {
      cars = cars.Where(c => c.Brand.Equals(query.Brand, StringComparison.OrdinalIgnoreCase));
    }

    if (query.FuelType.HasValue)
    {
      cars = cars.Where(c => c.FuelType == query.FuelType.Value);
    }

    var carDtos = new List<CarDto>();
    foreach (var car in cars)
    {
      var reviews = await _reviewRepository.GetReviewsByCarIdAsync(car.Id, cancellationToken);
      var reviewsList = reviews.ToList();
      double? averageRating = reviewsList.Any()
          ? reviewsList.Average(r => r.Rating)
          : null;

      carDtos.Add(new CarDto
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
        ReviewsCount = reviewsList.Count
      });
    }

    return carDtos;
  }
}
