using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Morent.Core.Helpers;

public class Security
{
  public static byte[] GenerateSalt(int size = 16)
  {
    byte[] salt = new byte[size];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }
    return salt;
  }

  public static string HashPassword(string password, byte[] salt)
  {
    using (var hmac = new HMACSHA512(salt))
    {
      var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      return Convert.ToBase64String(hash);
    }
  }

  public static bool VerifyPassword(string password, string hashedPassword, byte[] salt)
  {
    var hash = HashPassword(password, salt);
    return hash == hashedPassword;
  }

  public static string GenerateJwtToken(string username, string role, string key)
  {
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role)
    };

    var token = new JwtSecurityToken(
        issuer: null,
        audience: null,
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
