using System;

namespace Morent.Application.Features.Auth.DTOs;

public class RefreshTokenRequest
{
  public required string RefreshToken { get; set; } = null!;
}
