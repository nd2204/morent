using System;
using Morent.Application.Extensions;
using Morent.Application.Features.Car.DTOs;

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
      cars = cars.Where(c => c.CarModel.Brand.Equals(query.Brand, StringComparison.OrdinalIgnoreCase));
    }

    if (query.FuelType.HasValue)
    {
      cars = cars.Where(c => c.CarModel.FuelType == query.FuelType.Value);
    }

    var carDtos = new List<CarDto>();
    foreach (var car in cars)
    {
      var reviews = await _reviewRepository.GetReviewsByCarIdAsync(car.Id, cancellationToken);
      var reviewsList = reviews.ToList();
      double? averageRating = reviewsList.Any()
          ? reviewsList.Average(r => r.Rating)
          : null;

      carDtos.Add(car.ToCarDto());
    }

    return carDtos;
  }
}
