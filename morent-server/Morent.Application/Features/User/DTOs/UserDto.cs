using System;

namespace Morent.Application.Features.User.DTOs;

public class UserDto
{
  public Guid UserId { get; set; }
  public string Name { get; set; } = null!;
  public string Email { get; set; } = null!;
}
