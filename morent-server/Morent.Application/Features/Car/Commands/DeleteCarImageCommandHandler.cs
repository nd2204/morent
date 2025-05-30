using System;

namespace Morent.Application.Features.Car.Commands;

public class DeleteCarImageCommandHandler(ICarImageService carImageService) : ICommandHandler<DeleteCarImageCommand, Result>
{
  private readonly ICarImageService _carImageService = carImageService;
  public async Task<Result> Handle(DeleteCarImageCommand request, CancellationToken cancellationToken)
  {
    return await _carImageService.DeleteCarImageAsync(request.CarId, request.ImageId);
  }
}