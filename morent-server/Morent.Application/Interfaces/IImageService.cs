using System;

namespace Morent.Application.Interfaces;

public interface IImageService
{
  Task<ImageUploadResponse> UploadImageAsync(ImageUploadRequest request);
  Task<bool> DeleteImageAsync(Guid imageId);
  Task<ImageResult> GetImageByIdAsync(Guid imageId);
}
