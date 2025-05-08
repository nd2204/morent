using System;

namespace Morent.Application.Features.DTOs;

public class ImageStorageResult
{
  public bool Success { get; set; }
  public string Path { get; set; } = null!;
  public string Url { get; set; } = null!;
  public string Error { get; set; } = null!;
}
