using System;

namespace Morent.Application.Features.Auth;

public class RevokeTokenCommandHandler : ICommandHandler<RevokeTokenCommand, bool>
{
  private readonly IUserRepository _userRepository;

  public RevokeTokenCommandHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByIdAsync(request.userId);

    if (user == null)
      return false;

    // Revoke the specific refresh token
    if (!string.IsNullOrEmpty(request.refreshToken))
    {
      user.RevokeRefreshToken(request.refreshToken);
    }
    else
    {
      // If no specific token provided, revoke all tokens (logout from all devices)
      user.RevokeAllRefreshTokens();
    }

    await _userRepository.UpdateAsync(user);
    return true;
  }

}
