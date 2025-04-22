using Morent.Application.Models;
using Morent.Core.MorentUserAggregate;

namespace Morent.Application.Interfaces;

public interface IUserService
{
  Task<Result<MorentUser>> GetUserByIdAsync(Guid UserId);
  Task<Result<MorentUser>> GetUserByUsernameOrEmail(string usernameOrEmail);
  Task<Result<MorentUser>> CreateUserAsync(string username, string email, string passwordHash, byte[] salt);
  Task<Result> UpdateRefreshToken(Guid UserId, RefreshTokenDetails refreshToken);
  Task<bool> IsUsernameUniqueAsync(string username);
  Task<bool> IsEmailUniqueAsync(string email);
}
