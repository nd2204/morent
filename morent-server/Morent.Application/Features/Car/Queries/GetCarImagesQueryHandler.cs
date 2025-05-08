using System;

namespace Morent.Application.Features.Car.Queries;

public class GetCarImagesQueryHandler : IQueryHandler<GetCarImagesQuery, Result<IEnumerable<CarImageDto>>>
{
  private readonly ICarImageService _carImageService;

  public GetCarImagesQueryHandler(ICarImageService carImageService)
  {
    _carImageService = carImageService;
  }

  public async Task<Result<IEnumerable<CarImageDto>>> Handle(GetCarImagesQuery request, CancellationToken cancellationToken)
  {
    return await _carImageService.GetCarImagesAsync(request.CarId);
  }
}
