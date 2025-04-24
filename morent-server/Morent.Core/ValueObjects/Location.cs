using Morent.Core.Exceptions;

namespace Morent.Core.ValueObjects;

public class Location : ValueObject
{
  public string Address { get; private set; }
  public string City { get; private set; }
  public string Country { get; private set; }

  private Location() { } // For EF Core

  public Location(string address, string city, string country)
  {
    Guard.Against.NullOrEmpty(address, nameof(address));
    Guard.Against.NullOrEmpty(city, nameof(city));
    Guard.Against.NullOrEmpty(country, nameof(country));

    Address = address;
    City = city;
    Country = country;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Address;
    yield return City;
    yield return Country;
  }

  public override string ToString() => $"{Address}, {City}, {Country}";
}
