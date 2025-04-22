using System.ComponentModel.DataAnnotations;

namespace Morent.Application.DTOs;

public class RegisterRequest
{
  [Required]
  public string Username { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }

  [Required]
  [MinLength(8)]
  public string Password { get; set; }

  [Required]
  [Compare("Password")]
  public string ConfirmPassword { get; set; }
}