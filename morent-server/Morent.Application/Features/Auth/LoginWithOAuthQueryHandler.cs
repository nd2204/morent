using System;

namespace Morent.Application.Features.Auth;

public class LoginWithOAuthQueryHandler : IQueryHandler<LoginWithOAuthQuery, AuthResponse>
{
  private readonly IAuthService _authService;

  public LoginWithOAuthQueryHandler(IAuthService authService)
  {
    _authService = authService;
  }

  public async Task<AuthResponse> Handle(LoginWithOAuthQuery query, CancellationToken cancellationToken)
  {
    var request = new OAuthLoginRequest
    {
      Provider = query.Provider,
      ProviderToken = query.ProviderToken
    };

    return await _authService.AuthenticateWithOAuthAsync(request, cancellationToken);
  }
}