namespace Morent.Core.Interfaces;

public interface IUserService
{
  Task<Result<MorentUser>> GetUserByIdAsync(Guid id);
  Task<Result<MorentUser>> GetUserByCredentialAsync(string usernameOrEmail, string password);
  Task<Result<MorentUser>> CreateUserAsync(string username, string email, string password);
  Task<bool> IsUsernameUniqueAsync(string username);
  Task<bool> IsEmailUniqueAsync(string email);
}
