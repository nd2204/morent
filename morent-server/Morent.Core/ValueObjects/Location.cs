using Morent.Core.Exceptions;

namespace Morent.Core.ValueObjects;

public class Location : ValueObject
{
  public string? Address { get; private set; }
  public string? City { get; private set; }
  public string? Country { get; private set; }
  public double Longitude { get; private set; }
  public double Latitude { get; private set; }

  private Location() { } // For EF Core

  private Location(string? address, string? city, string? country, double longitude, double latitude)
  {
    Address = address;
    City = city;
    Country = country;
    Longitude = longitude;
    Latitude = latitude;
  }

  public static Result<Location> Create(string? address, string? city, string? country, double longitude, double latitude)
  {
    var validateResult = VerifyLocation(longitude, latitude);
    if (!validateResult.IsSuccess)
      return validateResult;

    return Result.Success(new Location(address, city, country, longitude, latitude));
  }

  public static Result<Location> CreateGeoLocOnly(double longitude, double latitude)
  {
    var validateResult = VerifyLocation(longitude, latitude);
    if (!validateResult.IsSuccess)
      return validateResult;

    return Result.Success(new Location("", "", "", longitude, latitude));
  }
  private static Result VerifyLocation(double longitude, double latitude)
  {
    if (longitude < -180 || longitude > 180)
      return Result.Invalid(new ValidationError("longitude must be in the range from -180 to 180"));

    if (latitude < -90 || latitude > 90)
      return Result.Invalid(new ValidationError("latitude must be in the range from -90 to 90"));

    return Result.NoContent();
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Longitude;
    yield return Latitude;
  }
  public override string ToString() => $"{Address}, {City}, {Country}";
}
