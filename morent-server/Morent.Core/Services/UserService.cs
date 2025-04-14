using Morent.Core.Interfaces;
using Morent.Core.MorentUserAggregate.Specifications;

namespace Morent.Core.Services;

public class UserService : IUserService
{
  private readonly IRepository<MorentUser> repository_;
  private readonly ISecurityService securityService_;

  public UserService(IRepository<MorentUser> repository, ISecurityService securityService) {
    repository_ = repository;
    securityService_ = securityService;
  }

  public async Task<Result<MorentUser>> CreateUserAsync(string username, string email, string password)
  {
    Guard.Against.NullOrEmpty(username);
    Guard.Against.NullOrEmpty(email);
    Guard.Against.NullOrEmpty(password);

    if (!await IsUsernameUniqueAsync(username)) {
      return Result.Error("Username already exists");
    }
    if (!await IsEmailUniqueAsync(username)) {
      return Result.Error("Username already exists");
    }

    byte[] salt = ISecurityService.GenerateSalt();
    var passwordHash = securityService_.HashPassword(password, salt);
    var user = new MorentUser(
      username,
      username, // Display name defaults to username
      email,
      passwordHash,
      salt,
      null // Empty phone number
    );

    await repository_.AddAsync(user);
    return Result.Success(user);
  }

  public async Task<Result<MorentUser>> GetUserByCredentialAsync(string usernameOrEmail, string password)
  {
    Guard.Against.NullOrEmpty(usernameOrEmail);
    Guard.Against.NullOrEmpty(password);
    var spec = new UserByUsernameOrEmailSpec(usernameOrEmail);
    var user = await repository_.FirstOrDefaultAsync(spec);

    if (user == null ||
      !securityService_.VerifyPassword(password, user.PasswordHash, user.PasswordSalt)
    )
      return Result.Error("Wrong username or password");

    return Result.Success(user);
  }

  public async Task<Result<MorentUser>> GetUserByIdAsync(Guid id)
  {
    var user = await repository_.GetByIdAsync(id);
    return user != null
      ? Result.Success(user)
      : Result.NotFound();
  }

  public async Task<bool> IsEmailUniqueAsync(string email)
  {
    var spec = new UserByUsernameOrEmailSpec(email);
    return !await repository_.AnyAsync(spec);
  }

  public async Task<bool> IsUsernameUniqueAsync(string username)
  {
    var spec = new UserByUsernameOrEmailSpec(username);
    return !await repository_.AnyAsync(spec);
  }
}
