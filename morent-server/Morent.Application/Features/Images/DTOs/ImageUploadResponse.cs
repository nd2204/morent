using System;

namespace Morent.Application.Features.Images.DTOs;

public class ImageUploadResponse
{
  public bool Success { get; set; }
  public Guid ImageId { get; set; }
  public string FilePath { get; set; } = null!;
  public string Url { get; set; } = null!;
  public List<string> Errors { get; set; } = new List<string>();

  public static ImageUploadResponse Successful(Guid imageId, string filePath, string url)
  {
    return new ImageUploadResponse
    {
      Success = true,
      ImageId = imageId,
      FilePath = filePath,
      Url = url
    };
  }

  public static ImageUploadResponse Failed(List<string> errors)
  {
    return new ImageUploadResponse
    {
      Success = false,
      Errors = errors
    };
  }
}