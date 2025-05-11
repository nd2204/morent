using System;

namespace Morent.Application.Features.User.DTOs;

public class UserDto
{
  public required Guid UserId { get; set; }
  public required string ImageUrl { get; set; }
  public required string Name { get; set; } = null!;
  public required string Email { get; set; } = null!;
}
