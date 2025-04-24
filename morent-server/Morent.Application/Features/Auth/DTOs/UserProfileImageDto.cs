using System;

namespace Morent.Application.Features.Auth.DTOs;

public class UserProfileImageDto
{
  public Guid? ImageId { get; set; }
  public string Url { get; set; } = null!;
  public DateTime? UploadedAt { get; set; }
}
