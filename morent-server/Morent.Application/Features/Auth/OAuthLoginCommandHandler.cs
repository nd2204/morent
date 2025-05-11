using System;

namespace Morent.Application.Features.Auth;

public class OAuthLoginCommandHandler : ICommandHandler<OAuthLoginCommand, AuthResponse>
{
  private readonly IAuthService _authService;
  private readonly IOAuthService _OAuthService;
  private readonly IUserRepository _userRepository;

  public OAuthLoginCommandHandler(
      IAuthService authService,
      IOAuthService externalAuthService,
      IUserRepository userRepository)
  {
    _authService = authService;
    _OAuthService = externalAuthService;
    _userRepository = userRepository;
  }


  public async Task<AuthResponse> Handle(OAuthLoginCommand request, CancellationToken cancellationToken)
  {
    // Validate the external token
    var externalLoginInfo = await _OAuthService.ValidateExternalToken(
        request.Provider,
        request.IdToken);

    if (externalLoginInfo == null)
      throw new UnauthorizedAccessException("Invalid external authentication");

    // Find user by external provider info or email
    var user = await _userRepository.GetByOAuthInfoAsync(
        request.Provider,
        externalLoginInfo.ProviderKey);

    if (user == null)
    {
      // Check if user exists with same email
      user = await _userRepository.GetByEmailAsync(externalLoginInfo.Email);

      if (user == null)
      {
        // Create new external user
        user = MorentUser.CreateExternalUser(
            externalLoginInfo.FirstName + externalLoginInfo.LastName,
            externalLoginInfo.Email,
            Email.Create(externalLoginInfo.Email).Value,
            request.Provider,
            externalLoginInfo.ProviderKey).Value;

        await _userRepository.AddAsync(user);
      }
      else
      {
        // Link external provider to existing account
        user.AddExternalLogin(
            request.Provider,
            externalLoginInfo.ProviderKey);

        await _userRepository.UpdateAsync(user);
      }
    }

    // Generate tokens
    var authResponse = await _authService.GenerateAuthResponse(user);

    // Add refresh token
    var refreshToken = new RefreshToken(authResponse.RefreshToken, DateTime.UtcNow.AddDays(7));
    user.AddRefreshToken(refreshToken);
    // user.PurgeOldRefreshTokens(5);

    await _userRepository.UpdateAsync(user);

    return authResponse;
  }
}
