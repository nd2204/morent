using System;

namespace Morent.Application.Features.Auth;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<AuthResponse>>
{
  private readonly IAuthService _authService;
  private readonly IUserRepository _userRepository;

  public RefreshTokenCommandHandler(IAuthService authService, IUserRepository userRepository)
  {
    _authService = authService;
    _userRepository = userRepository;
  }

  public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
  {
    // Validate the refresh token
    var result = await _authService.ValidateRefreshTokenAsync(request.RefreshToken);

    if (result is null)
      throw new UnauthorizedAccessException("Invalid refresh token");

    var user = result.Value;

    // Revoke the used refresh token
    user.RevokeRefreshToken(request.RefreshToken);

    // Generate new tokens
    var response = await _authService.GenerateAuthResponse(user);

    // Add new refresh token to user
    user.AddRefreshToken(new RefreshToken(response.RefreshToken, DateTime.UtcNow.AddDays(7)));

    // Save changes
    await _userRepository.UpdateAsync(user, cancellationToken);

    return response;
  }
}
