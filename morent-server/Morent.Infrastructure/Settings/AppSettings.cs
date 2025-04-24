using System;

namespace Morent.Infrastructure.Settings;

public class AppSettings
{
  public string JwtSecret { get; set; } = null!;
  public string JwtIssuer { get; set; } = null!;
  public string JwtAudience { get; set; } = null!;
  public int JwtExpiryMinutes { get; set; }
  public int RefreshTokenExpiryDays { get; set; } = 7;
  public string GoogleClientId { get; set; } = null!;
  public string GoogleClientSecret { get; set; } = null!;
}
