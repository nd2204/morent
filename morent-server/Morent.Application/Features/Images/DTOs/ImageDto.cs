using System;

namespace Morent.Application.Features.Images.DTOs;

public class ImageDto
{
  public Guid Id { get; set; }
  public string Url { get; set; } = null!;
  public string ContentType { get; set; } = null!;
  public string FileName { get; set; } = null!;
  public DateTime UploadedAt { get; set; }
  public long Size { get; set; }
}
