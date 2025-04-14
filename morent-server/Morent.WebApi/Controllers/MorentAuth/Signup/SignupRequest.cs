using System;
using System.ComponentModel.DataAnnotations;

namespace Morent.WebApi.Controllers.MorentAuth.Signup;

public class SignupRequest
{
  public const string Route = "signup";

  [Required]
  public string Username { get; set; } = string.Empty;
  [Required]
  public string Password { get; set; } = string.Empty;
  [Required]
  public string Email { get; set; } = string.Empty;
}
