using System;

namespace Morent.Application.Features.Auth.DTOs;

public class RefreshTokenRequest
{
  public string RefreshToken { get; set; } = null!;
}
