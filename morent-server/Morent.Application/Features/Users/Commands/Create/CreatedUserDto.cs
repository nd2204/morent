using Morent.Core.MorentUserAggregate;

namespace Morent.Application.Features.Users.Commands.Create;

public class CreatedUserDto
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string Role { get; set; } = null!;
}