using System;

namespace Morent.Application.Features.Auth.DTOs;

public class AuthResponse
{
  public required string AccessToken { get; set; } = null!;
  public required string RefreshToken { get; set; } = null!;
  public required DateTime ExpiresAt { get; set; }
  public required UserDto User { get; set; } = null!;
}