using System;
using System.Reflection.Metadata.Ecma335;

namespace Morent.Application.Features.Car.Commands;

public class SetCarPrimaryImageCommandHandler(
  ICarImageService _carImageService)
  : ICommandHandler<SetCarPrimaryImageCommand, Result<CarImageDto>>
{
  public async Task<Result<CarImageDto>> Handle(SetCarPrimaryImageCommand request, CancellationToken cancellationToken)
  {
    return await _carImageService.SetPrimaryImageAsync(request.CarId, request.ImageId);
  }
}