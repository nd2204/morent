using Microsoft.AspNetCore.Http;

namespace Morent.Application.Features.Car.Commands;

public record class UploadCarImageCommand(
  Guid CarId,
  IFormFile File,
  string Url,
  bool SetAsPrimary) : ICommand<Result<CarImageDto>>;
