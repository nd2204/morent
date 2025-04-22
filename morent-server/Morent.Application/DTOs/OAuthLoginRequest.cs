using System;
namespace Morent.Application.DTOs;

public class OAuthLoginRequest
{
  [Required]
  public string ProviderToken { get; set; } = null!;
  [Required]
  public OAuthProvider Provider { get; set; }
}