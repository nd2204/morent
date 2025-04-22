using System;

namespace Morent.Application.Features.Auth;

public class RegisterUserCommand : ICommand<AuthResponse>
{
  public string? Name { get; set; }
  public string Username { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
}