namespace Morent.Core.ValueObjects;

public class RefreshToken : ValueObject
{
  public string Token { get; private set; }
  public DateTime ExpiresAt { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime? RevokedAt { get; private set; }

  public bool IsRevoked => RevokedAt != null || DateTime.UtcNow >= ExpiresAt;
  public bool IsActive => !IsRevoked;

  private RefreshToken() { }

  public RefreshToken(string token, DateTime expiresAt)
  {
    Token = token;
    ExpiresAt = expiresAt;
    CreatedAt = DateTime.UtcNow;
  }

  public void Revoke()
  {
    if (!IsRevoked)
    {
      RevokedAt = DateTime.UtcNow;
    }
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Token;
    yield return ExpiresAt;
    yield return CreatedAt;
    yield return RevokedAt ?? DateTime.MinValue;
  }
}