namespace Morent.Application.Features.Users;

public class UserDto
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string Username { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string AccessToken { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;
  public string ExpiresIn { get; set; } = null!;

  public void SetSecureToken(string accessToken, string refreshToken, string expiresIn) {
    AccessToken = accessToken;
    RefreshToken = refreshToken;
    ExpiresIn = expiresIn;
  }
}