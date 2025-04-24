using System;

namespace Morent.Application.Interfaces;

public interface IImageStorage
{
  Task<ImageStorageResult> StoreImageAsync(Stream imageStream, string fileName, string contentType);
  Task<bool> DeleteImageAsync(string path);
  string GetImageUrl(string path);
}
