using System.ComponentModel.DataAnnotations;

namespace Morent.Application.Features.Auth.DTOs;

public class RegisterRequest
{
  public string? Name { get; set; } = null!;

  [Required]
  public string Username { get; set; } = null!;

  [Required]
  [EmailAddress]
  public string Email { get; set; } = null!;

  [Required]
  [MinLength(8)]
  public string Password { get; set; } = null!;
}