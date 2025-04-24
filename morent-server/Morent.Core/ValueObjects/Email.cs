namespace Morent.Core.ValueObjects;

public class Email : ValueObject
{
  public string Value { get; private set; }

  private Email() { Value = "N/a"; }

  public Email(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      throw new ArgumentException("Email cannot be empty", nameof(value));

    if (!IsValidEmail(value)) 
        throw new ArgumentException("Email is not in valid format.");

    Value = value;
  }

  private static bool IsValidEmail(string email)
  {
    try
    {
      var addr = new System.Net.Mail.MailAddress(email);
      return addr.Address == email;
    }
    catch
    {
      return false;
    }
  }

  public override string ToString() => Value;

  public override int GetHashCode() => Value.GetHashCode();

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value.ToLowerInvariant();
  }
}