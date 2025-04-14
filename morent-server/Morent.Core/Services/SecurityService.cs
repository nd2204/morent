using System;
using System.Security.Cryptography;
using System.Text;
using Morent.Core.Interfaces;

namespace Morent.Core.Services;

public class SecurityService : ISecurityService
{
  public string HashPassword(string password, byte[] salt)
  {
    using (var hmac = new HMACSHA512(salt))
    {
      var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      return Convert.ToBase64String(hash);
    }
  }

  public bool VerifyPassword(string password, string hash, byte[] salt)
  {
    return hash == HashPassword(password, salt);
  }
}
