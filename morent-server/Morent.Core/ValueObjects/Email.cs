using System.Text.RegularExpressions;

namespace Morent.Core.ValueObjects;

public class Email : ValueObject
{
  public string Value { get; private set; }

  private Email() {}

  private Email(string value)
  {
    Value = value;
  }

  public static Result<Email> Create(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Result.Invalid(new ValidationError("Email", "Email cannot be empty"));

    if (!IsValidEmail(value)) 
      return Result.Invalid(new ValidationError("Email", "Email is not in valid format."));

    return Result.Success(new Email(value));
  }

  private static bool IsValidEmail(string email)
  {
    var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$");
    return regex.IsMatch(email);
  }

  public override string ToString() => Value;

  public override int GetHashCode() => Value.GetHashCode();

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value.ToLowerInvariant();
  }
}