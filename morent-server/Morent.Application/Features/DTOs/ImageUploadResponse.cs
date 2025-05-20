using System;

namespace Morent.Application.Features.DTOs;

public class ImageUploadResponse
{
  public ImageUploadResponse(Guid imageId, string fileName, string filePath, string url)
  {
    ImageId = imageId;
    FilePath = filePath;
    FileName = fileName;
    Url = url;
  }

  public Guid ImageId { get; set; }
  public string FilePath { get; set; } = null!;
  public string FileName { get; set; } = null!;
  public string Url { get; set; } = null!;
}