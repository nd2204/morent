using System;

namespace Morent.Application.Features.Auth;

public class LoginUserQuery : IQuery<AuthResponse>
{
  public string LoginId { get; set; } = null!;
  public string Password { get; set; } = null!;
}
