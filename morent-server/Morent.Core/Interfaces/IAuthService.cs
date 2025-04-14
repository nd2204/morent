using System;

namespace Morent.Core.Interfaces;

public interface IAuthService
{
  Task<Result<MorentUser>> LoginAsync(string usernameOrEmail, string password);
  Task<Result<MorentUser>> SignupAsync(string username, string email, string password);
}
