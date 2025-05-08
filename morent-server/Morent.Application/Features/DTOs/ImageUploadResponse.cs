using System;

namespace Morent.Application.Features.DTOs;

public class ImageUploadResponse
{
  public ImageUploadResponse(Guid imageId, string filePath, string url)
  {
    ImageId = imageId;
    FilePath = filePath;
    Url = url;
  }

  public Guid ImageId { get; set; }
  public string FilePath { get; set; } = null!;
  public string Url { get; set; } = null!;
}