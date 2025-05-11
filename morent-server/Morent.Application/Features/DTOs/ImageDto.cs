using System;

namespace Morent.Application.Features.DTOs;

public class ImageDto
{
  public required Guid Id { get; set; }
  public required string Url { get; set; } = null!;
  public required string ContentType { get; set; } = null!;
  public required string FileName { get; set; } = null!;
  public required DateTime UploadedAt { get; set; }
  public required long Size { get; set; }
}
