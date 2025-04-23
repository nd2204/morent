namespace Morent.Application.Interfaces;
public interface IAuthService
{
  string GenerateJwtToken(MorentUser user);
  RefreshToken GenerateRefreshToken();
  AuthResponse GenerateAuthResponse(MorentUser user);
  string HashPassword(string password);
  bool VerifyPassword(string password, string passwordHash);

  Task<AuthResponse> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default);
  Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

  Task<MorentUser?> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
  Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}