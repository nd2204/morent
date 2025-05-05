using System;

namespace Morent.Application.Interfaces;

public interface IImageService
{
  Task<Result<ImageUploadResponse>> UploadImageAsync(ImageUploadRequest request);
  Task<Result> DeleteImageAsync(Guid imageId);
  Task<Result<ImageDto>> GetImageByIdAsync(Guid imageId);
}
