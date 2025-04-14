using System;
using Morent.Core.MorentUserAggregate;

namespace Morent.WebApi.Controllers.MorentAuth.Login;

public class LoginResponse
{
  public LoginResponse(Guid id, string accessTkn, string refreshTkn)
  {
    UserId = id.ToString();
    AccessToken = accessTkn;
    RefreshToken = refreshTkn;
  }

  public string UserId { get; set; } = string.Empty;
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
}
