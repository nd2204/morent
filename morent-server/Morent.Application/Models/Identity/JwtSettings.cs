using System;

namespace Morent.Application.Models.Identity;

public class JwtSettings
{
  public string Secret { get; set; } = null!;
  public string Issuer { get; set; } = null!;
  public string Audience { get; set; } = null!;
  public double DurationInMinutes { get; set; }
}
