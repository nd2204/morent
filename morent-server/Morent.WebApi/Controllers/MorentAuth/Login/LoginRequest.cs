using System.ComponentModel.DataAnnotations;

namespace Morent.WebApi.Controllers.MorentAuth.Login;

public class LoginRequest
{
  public const string Route = "login";

  [Required]
  public string UsernameOrEmail { get; set; } = string.Empty;
  [Required]
  public string Password { get; set; } = string.Empty;
}
