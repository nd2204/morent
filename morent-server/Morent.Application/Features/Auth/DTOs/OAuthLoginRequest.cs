using System;
using System.ComponentModel.DataAnnotations;

namespace Morent.Application.Features.Auth.DTOs;

public class OAuthLoginRequest
{
  [Required]
  public string ProviderToken { get; set; } = null!;
  [Required]
  public string Provider { get; set; } = null!;
}