namespace Morent.Core.ValueObjects;

public class Location : ValueObject
{
  public string Address { get; private set; }
  public string City { get; private set; }
  public string State { get; private set; }
  public string ZipCode { get; private set; }
  public string Country { get; private set; }
  public double? Latitude { get; private set; }
  public double? Longitude { get; private set; }

  private Location() { } // For EF Core

  public Location(string address, string city, string state, string zipCode, string country, double? latitude = null, double? longitude = null)
  {
    Guard.Against.NullOrEmpty(address, nameof(address));
    Guard.Against.NullOrEmpty(city, nameof(city));
    Guard.Against.NullOrEmpty(country, nameof(country));

    Address = address;
    City = city;
    State = state;
    ZipCode = zipCode;
    Country = country;
    Latitude = latitude;
    Longitude = longitude;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Address;
    yield return City;
    yield return State;
    yield return ZipCode;
    yield return Country;
    if (Latitude.HasValue) yield return Latitude.Value;
    if (Longitude.HasValue) yield return Longitude.Value;
  }

  public override string ToString() => $"{Address}, {City}, {State} {ZipCode}, {Country}";
}
