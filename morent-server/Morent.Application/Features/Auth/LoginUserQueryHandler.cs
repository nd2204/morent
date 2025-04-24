using System;

namespace Morent.Application.Features.Auth;

public class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, AuthResponse>
{
  private readonly IAuthService _authService;

  public LoginUserQueryHandler(IAuthService authService)
  {
    _authService = authService;
  }

  public async Task<AuthResponse> Handle(LoginUserQuery query, CancellationToken cancellationToken)
  {
    var request = new LoginRequest
    {
      LoginId = query.LoginId,
      Password = query.Password
    };

    return await _authService.AuthenticateAsync(request, cancellationToken);
  }
}