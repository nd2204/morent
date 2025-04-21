using Morent.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ardalis.Result;
using Microsoft.Extensions.Options;
using Morent.Application.Features.Users;
using Morent.Application.Interfaces;
using Morent.Application.Extensions;
using Morent.Application.Models.Identity;

namespace Morent.Infrastructure.Services;

public class AuthService : IAuthService
{
  private readonly IUserService userService_;
  private readonly JwtSettings jwtSetting_;

  public AuthService(
    IUserService userService,
    IOptions<JwtSettings> jwtSetting)
  {
    userService_ = userService;
    jwtSetting_ = jwtSetting.Value;
  }

  public async Task<Result<UserDto>> LoginAsync(string usernameOrEmail, string password)
  {
    var result = await userService_.GetUserByUsernameOrEmail(usernameOrEmail);
    var user = result.Value;
    if (user == null)
    {
      return Result.NotFound($"User with username or email {usernameOrEmail} not found.");
    }

    if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt)) {
      return Result.Error("Wrong username or password");
    }

    var userDto = user.ToDto();
    userDto.SetSecureToken(
      GenerateToken(user),
      GenerateRefreshToken(),
      jwtSetting_.DurationInMinutes.ToString()
      );

    return Result.Success(userDto);
  }

  public async Task<Result<UserDto>> SignupAsync(string username, string password, string email)
  {
    var salt = GenerateSalt();
    var hashedPassword = HashPassword(password, salt);
    var result = await userService_.CreateUserAsync(username, email, hashedPassword, salt);
    
    if (result.IsError()) {
      return Result.Error(result.Errors.First() ?? "Unexpected error.");
    }

    var newUser = result.Value.ToDto();

    return Result.Success(newUser);
  }

  private static string HashPassword(string password, byte[] salt)
  {
    using (var hmac = new HMACSHA512(salt))
    {
      var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      return Convert.ToBase64String(hash);
    }
  }

  private static bool VerifyPassword(string password, string hash, byte[] salt)
  {
    return hash == HashPassword(password, salt);
  }


  private static byte[] GenerateSalt(int size = 16)
  {
    byte[] salt = new byte[size];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }
    return salt;
  }

  private string GenerateToken(MorentUser user)
  {
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Name),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("uid", user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var securityKey = new SymmetricSecurityKey(
     Encoding.UTF8.GetBytes(jwtSetting_.Secret)
     );
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: jwtSetting_.Issuer,
        audience: jwtSetting_.Audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(jwtSetting_.DurationInMinutes),
        signingCredentials: credentials
        );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private string GenerateRefreshToken() {
    throw new NotImplementedException();
  }
}