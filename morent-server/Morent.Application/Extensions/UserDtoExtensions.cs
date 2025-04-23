using Morent.Application.Features.Users;
using Morent.Core.MorentUserAggregate;
using Morent.Core.ValueObjects;

namespace Morent.Application.Extensions;

public static class UserMappingExtensions
{
  public static UserDto ToDto(this MorentUser user)
  {
    return new UserDto
    {
      Id = user.Id.ToString(),
      Name = user.Name,
      Username = user.Username,
      Email = user.Email.ToString(),
      AccessToken = "",
      RefreshToken = "",
      ExpiresIn = "",
    };
  }
}
