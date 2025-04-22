namespace Morent.Application.Features.Auth;

public class LoginWithOAuthQuery : IQuery<AuthResponse>
{
  public OAuthProvider Provider { get; set; }
  public string ProviderToken { get; set; } = null!;
}
