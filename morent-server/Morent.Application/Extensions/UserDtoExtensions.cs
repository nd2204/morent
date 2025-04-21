using System;
using Morent.Application.Features.Users;
using Morent.Core.MorentUserAggregate;

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
      Email = user.Email,
      AccessToken = "",
      RefreshToken = "",
      ExpiresIn = "",
    };
  }
}
