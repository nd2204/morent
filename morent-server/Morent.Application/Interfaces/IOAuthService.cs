using System;

namespace Morent.Application.Interfaces;
public class OAuthLoginInfo
{
  public string Email { get; set; } = null!;
  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string ProviderKey { get; set; } = null!;
}

public interface IOAuthService
{
  Task<OAuthLoginInfo?> ValidateExternalToken(string provider, string idToken);
}
