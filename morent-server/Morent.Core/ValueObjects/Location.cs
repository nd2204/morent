using Morent.Core.Exceptions;

namespace Morent.Core.ValueObjects;

public class Location : ValueObject
{
  public string Address { get; private set; }
  public string City { get; private set; }
  public string Country { get; private set; }

  private Location() { } // For EF Core

  private Location(string address, string city, string country)
  {
    Guard.Against.NullOrEmpty(address, nameof(address));
    Guard.Against.NullOrEmpty(city, nameof(city));
    Guard.Against.NullOrEmpty(country, nameof(country));

    Address = address;
    City = city;
    Country = country;
  }

  public static Result<Location> Create(string address, string city, string country)
  {
    if (string.IsNullOrEmpty(address))
      return Result.Invalid(new ValidationError(nameof(address), "must not be null or empty"));

    if (string.IsNullOrEmpty(city))
      return Result.Invalid(new ValidationError(nameof(city), "must not be null or empty"));

    if (string.IsNullOrEmpty(country))
      return Result.Invalid(new ValidationError(nameof(country), "must not be null or empty"));

    return Result.Success(new Location(address, city, country));
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Address;
    yield return City;
    yield return Country;
  }

  public override string ToString() => $"{Address}, {City}, {Country}";
}
