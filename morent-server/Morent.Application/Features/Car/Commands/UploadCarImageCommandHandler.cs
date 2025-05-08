using System;

namespace Morent.Application.Features.Car.Commands;

public class UploadCarImageCommandHandler : ICommandHandler<UploadCarImageCommand, Result<CarImageDto>>
{
  private readonly ICarImageService _carImageService;

  public UploadCarImageCommandHandler(ICarImageService carImageService)
  {
    _carImageService = carImageService;
  }

  public async Task<Result<CarImageDto>> Handle(UploadCarImageCommand request, CancellationToken cancellationToken)
  {
    try
    {
      using var stream = request.File.OpenReadStream();

      var uploadRequest = new ImageUploadRequest
      {
        ImageData = stream,
        FileName = request.File.FileName,
        ContentType = request.File.ContentType
      };

      var result = await _carImageService.AddCarImageAsync(request.CarId, uploadRequest, request.SetAsPrimary);
      return result;
    }
    catch (Exception ex)
    {
      return Result.Error(ex.Message);
    }
  }
}
