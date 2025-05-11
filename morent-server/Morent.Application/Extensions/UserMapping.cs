using System;

namespace Morent.Application.Extensions;

public static class UserMapping
{
  public static UserDto ToDto(this MorentUser user)
  {
    return new UserDto {
      UserId = user.Id,
      Name = user.Name,
      ImageUrl = "",
      Email = user.Email.ToString()
    };
  }
}
