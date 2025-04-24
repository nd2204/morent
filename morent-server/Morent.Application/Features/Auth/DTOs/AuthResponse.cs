using System;

namespace Morent.Application.Features.Auth.DTOs;

public class AuthResponse
{
  public string AccessToken { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;
  public DateTime ExpiresAt { get; set; }
  public UserDto User { get; set; } = null!;
}