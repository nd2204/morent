using System;
using Morent.Core.MediaAggregate;

namespace Morent.Application.Interfaces;

public interface IImageStorage
{
  Task<ImageStorageResult> StoreImageAsync(Stream imageStream, string fileName, string contentType);
  Task CleanupOrphanedImages(IRepository<MorentImage> images);
  Task<bool> DeleteImageAsync(string path);
  string GetImageUrl(string path);
}
