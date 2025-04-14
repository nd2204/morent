// using System;
// using Morent.Core.Interfaces;
// using Morent.Core.MorentUserAggregate;
// using Morent.Core.MorentUserAggregate.Specifications;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Security.Cryptography;
// using System.Text;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.Extensions.Configuration;

// namespace Morent.Core.Services;

// public class AuthServices : IAuthServices
// {
//   private readonly IRepository<MorentUser> repository_;

//   public AuthServices(IRepository<MorentUser> repo)
//   {
//     repository_ = repo;
//   }

//   public async Task<Result<string>> LoginAsync(string usernameOrEmail, string password)
//   {
//     var spec = new UserByUsernameOrEmailSpec(usernameOrEmail);
//     var user = await repository_.FirstOrDefaultAsync(spec);
//     if (user is null) {
//       return Result.NotFound("Username or Email not exists");
//     }

//     return Result.Success(GenerateJwtToken(user.Username, MorentUserRole.User.ToString()));
//   }

//   public async Task<Result<MorentUser>> SignupAsync(string username, string password, string email)
//   {
//     var spec = new UserByUsernameOrEmailSpec(username, email);
//     if (await repository_.AnyAsync(spec)) {
//       return Result.Unavailable("Username or Email already exists");
//     }

//     var salt = GenerateSalt();
//     var hashedPassword = HashPassword(password, salt);
//     var newUser = new MorentUser(username, username, email, "", hashedPassword, salt);

//     await repository_.AddAsync(newUser);
//     await repository_.SaveChangesAsync();

//     return Result.Success(newUser);
//   }

//   public static string HashPassword(string password, byte[] salt)
//   {
//   }

//   public static bool VerifyPassword(string password, string hashedPassword, byte[] salt)
//   {
//     var hash = HashPassword(password, salt);
//     return hash == hashedPassword;
//   }

//   public string GenerateJwtToken(string username, string role)
//   {
//     var securityKey = new SymmetricSecurityKey(
//       Encoding.UTF8.GetBytes()
//       );
//     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//     var claims = new[]
//     {
//         new Claim(ClaimTypes.Name, username),
//         new Claim(ClaimTypes.Role, role)
//     };

//     var token = new JwtSecurityToken(
//         issuer: null,
//         audience: null,
//         claims: claims,
//         expires: DateTime.Now.AddMinutes(30),
//         signingCredentials: credentials
//         );

//     return new JwtSecurityTokenHandler().WriteToken(token);
//   }
// }