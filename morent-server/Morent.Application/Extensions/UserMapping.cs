using System;

namespace Morent.Application.Extensions;

public static class UserMapping
{
  public static UserDto ToDto(this MorentUser user)
  {
    return new UserDto {
      UserId = user.Id,
      Name = user.Name,
      Email = user.Email.ToString()
    };
  }
}
