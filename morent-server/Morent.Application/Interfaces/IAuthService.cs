namespace Morent.Application.Interfaces;
public interface IAuthService
{
  Task<AuthResponse> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default);
  Task<AuthResponse> AuthenticateWithOAuthAsync(OAuthLoginRequest request, CancellationToken cancellationToken = default);
  Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
  Task<AuthResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
  Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}