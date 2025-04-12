using System;
using System.Security.Cryptography;

namespace Scripts;

public class SecretGenerator
{
  public static string GenerateRandomHexString()
  {
    return RandomNumberGenerator.GetHexString(64, true);
  }
}
