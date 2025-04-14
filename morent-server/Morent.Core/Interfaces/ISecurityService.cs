using System.Security.Cryptography;

namespace Morent.Core.Interfaces;

public interface ISecurityService
{
  string HashPassword(string password, byte[] salt);
  bool VerifyPassword(string password, string hash, byte[] salt);

  static byte[] GenerateSalt(int size = 16)
  {
    byte[] salt = new byte[size];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }
    return salt;
  }
}
