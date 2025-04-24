using System;
using Morent.Application.Extensions;
using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Car;

public class GetCarByIdQueryHandler : IQueryHandler<GetCarByIdQuery, CarDetailDto>
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

  public async Task<CarDetailDto> Handle(GetCarByIdQuery query, CancellationToken cancellationToken)
  {
    var car = await _carRepository.GetCarWithReviewsAsync(query.Id, cancellationToken);
    if (car == null)
      throw new ApplicationException($"Car with ID {query.Id} not found");

    return car.ToCarDetailDto();
  }
}