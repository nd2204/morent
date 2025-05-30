using System;

namespace Morent.Application.Features.DTOs;

public class ImageUploadRequest
{
  public Stream ImageData { get; set; } = null!;
  public string FileName { get; set; } = null!;
  public string ContentType { get; set; } = null!;
}
