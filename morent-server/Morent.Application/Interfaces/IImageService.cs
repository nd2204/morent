using System;

namespace Morent.Application.Interfaces;

public interface IImageService
{
  Task<Result<ImageUploadResponse>> UploadImageAsync(ImageUploadRequest request);
  Task<Result<ImageUploadResponse>> UploadImageAsync(string imageUrl);
  Task<Result<ImageUploadRequest>> FetchImageAsUploadRequest(string imageUrl);
  Task<Result> DeleteImageAsync(Guid imageId);
  Task<Result<ImageDto>> GetImageByIdAsync(Guid imageId);
  Task<Result<ImageDto>> GetPlaceHolderImageAsync();
}
