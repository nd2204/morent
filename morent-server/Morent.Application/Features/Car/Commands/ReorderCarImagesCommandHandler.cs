using System;

namespace Morent.Application.Features.Car.Commands;

public class ReorderCarImagesCommandHandler(ICarImageService _carImageService)
  : ICommandHandler<ReorderCarImagesCommand, Result>
{
  public async Task<Result> Handle(ReorderCarImagesCommand request, CancellationToken cancellationToken)
  {
    if (request.newOrder == null || request.newOrder.Count() < 1)
      return Result.Invalid(new ValidationError("New orders must not be null or empty"));

    return await _carImageService.ReorderCarImagesAsync(request.CarId, request.newOrder);
  }
}
