// using Morent.Application.Models;
// using Morent.Application.Interfaces;
// using Morent.Core.MorentUserAggregate;
// using Morent.Core.MorentUserAggregate.Specifications;

// namespace Morent.Application.Services;

// public class UserService : IUserService
// {
//   private readonly IRepository<MorentUser> repository_;

//   public UserService(IRepository<MorentUser> repository) {
//     repository_ = repository;
//   }

//   public async Task<Result<MorentUser>> CreateUserAsync(
//     string username, string email, string passwordHash)
//   {
//     Guard.Against.NullOrEmpty(username);
//     Guard.Against.NullOrEmpty(email);
//     Guard.Against.NullOrEmpty(passwordHash);

//     if (!await IsUsernameUniqueAsync(username)) {
//       return Result.Error("Username already exists");
//     }
//     if (!await IsEmailUniqueAsync(username)) {
//       return Result.Error("Username already exists");
//     }


//     var user = new MorentUser(
//       username,
//       username, // Display name defaults to username
//       email,
//       passwordHash,
//       salt,
//       null // Empty phone number
//     );

//     await repository_.AddAsync(user);
//     return Result.Success(user);
//   }

//   public async Task<Result<MorentUser>> GetUserByUsernameOrEmail(string usernameOrEmail)
//   {
//     Guard.Against.NullOrEmpty(usernameOrEmail);
//     var spec = new UserByUsernameOrEmailSpec(usernameOrEmail);
//     var user = await repository_.FirstOrDefaultAsync(spec);

//     if (user == null)
//       return Result.Error("Wrong username or password");

//     return Result.Success(user);
//   }

//   public async Task<Result<MorentUser>> GetUserByIdAsync(Guid id)
//   {
//     var user = await repository_.GetByIdAsync(id);
//     return user != null
//       ? Result.Success(user)
//       : Result.NotFound();
//   }

//   public async Task<bool> IsEmailUniqueAsync(string email)
//   {
//     var spec = new UserByUsernameOrEmailSpec(email);
//     return !await repository_.AnyAsync(spec);
//   }

//   public async Task<bool> IsUsernameUniqueAsync(string username)
//   {
//     var spec = new UserByUsernameOrEmailSpec(username);
//     return !await repository_.AnyAsync(spec);
//   }

//   public Task<Result> UpdateRefreshToken(Guid UserId, RefreshTokenDetails refreshToken)
//   {
//     throw new NotImplementedException();
//   }

//   public Task<Result<MorentUser>> CreateUserAsync(string username, string email, string passwordHash, byte[] salt)
//   {
//     throw new NotImplementedException();
//   }
// }
