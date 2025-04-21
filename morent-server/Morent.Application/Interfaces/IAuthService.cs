using System;
using Morent.Application.Features.Users;

namespace Morent.Application.Interfaces;

public interface IAuthService
{
  Task<Result<UserDto>> LoginAsync(string usernameOrEmail, string password);
  Task<Result<UserDto>> SignupAsync(string username, string email, string password);
}
