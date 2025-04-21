using System;
using Morent.Core.MorentUserAggregate;

namespace Morent.WebApi.Controllers.MorentAuth.Login;

public class LoginResponse
{
  public required string UserId { get; set; } = string.Empty;
  public required string Email { get; set; } = string.Empty;
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
  public string ExpiresIn { get; set; } = string.Empty;
}
