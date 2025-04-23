using System;

namespace Morent.Application.Features.Auth;

public record class RegisterUserCommand(
  string? Name,
  string Username,
  string Email,
  string Password) : ICommand<AuthResponse>;