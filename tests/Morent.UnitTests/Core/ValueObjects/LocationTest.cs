using Morent.Core.ValueObjects;
using System;
using Xunit;
using Shouldly;
using Ardalis.Result;

namespace Morent.UnitTests.Core.ValueObjects;

public class LocationTests
{
  [Fact]
  public void CreateGeoLocOnly_WithValidParameters_ShouldCreateLocation()
  {
    // Arrange
    double latitude = 40.7812;
    double longitude = -73.9665;

    // Act
    var locationResult = Location.CreateGeoLocOnly(longitude, latitude);

    // Assert
    locationResult.IsSuccess.ShouldBeTrue();
    var location = locationResult.Value;
    location.Latitude.ShouldBe(latitude);
    location.Longitude.ShouldBe(longitude);
    location.Address.ShouldBe("");
    location.City.ShouldBe("");
    location.Country.ShouldBe("");
  }

  [Theory]
  [InlineData(-91, 0, "Longitude must be in the range from -90 to 90")]
  [InlineData(91, 0, "Longitude must be in the range from -90 to 90")]
  [InlineData(0, -181, "Latitude must be in the range from -180 to 180")]
  [InlineData(0, 181, "Latitude must be in the range from -180 to 180")]
  public void CreateGeoLocOnly_WithInvalidParameters_ShouldReturnInvalidResult(double longitude, double latitude, string expectedError)
  {
    // Act
    var locationResult = Location.CreateGeoLocOnly(longitude, latitude);

    // Assert
    locationResult.Status.ShouldBe(ResultStatus.Invalid);
    locationResult.ValidationErrors.ShouldContain(error => error.ErrorMessage == expectedError);
  }

  [Fact]
  public void Create_WithValidParameters_ShouldCreateLocation()
  {
    // Arrange
    string address = "123 Main St";
    string city = "New York";
    string country = "USA";
    double latitude = 40.7812;
    double longitude = -73.9665;

    // Act
    var locationResult = Location.Create(address, city, country, longitude, latitude);

    // Assert
    locationResult.IsSuccess.ShouldBeTrue();
    var location = locationResult.Value;
    location.Address.ShouldBe(address);
    location.City.ShouldBe(city);
    location.Country.ShouldBe(country);
    location.Latitude.ShouldBe(latitude);
    location.Longitude.ShouldBe(longitude);
  }

  [Theory]
  [InlineData("123 Main St", "New York", "USA", -91, 0, "Longitude must be in the range from -90 to 90")]
  [InlineData("123 Main St", "New York", "USA", 91, 0, "Longitude must be in the range from -90 to 90")]
  [InlineData("123 Main St", "New York", "USA", 0, -181, "Latitude must be in the range from -180 to 180")]
  [InlineData("123 Main St", "New York", "USA", 0, 181, "Latitude must be in the range from -180 to 180")]
  public void Create_WithInvalidParameters_ShouldReturnInvalidResult(string address, string city, string country, double longitude, double latitude, string expectedError)
  {
    // Act
    var locationResult = Location.Create(address, city, country, longitude, latitude);

    // Assert
    locationResult.Status.ShouldBe(ResultStatus.Invalid);
    locationResult.ValidationErrors.ShouldContain(error => error.ErrorMessage == expectedError);
  }

  [Fact]
  public void ToString_ShouldReturnFormattedString()
  {
    // Arrange
    var locationResult = Location.Create("123 Main St", "New York", "USA", -73.9665, 40.7812);

    // Act & Assert
    locationResult.IsSuccess.ShouldBeTrue();
    var location = locationResult.Value;
    var result = location.ToString();
    result.ShouldBe("123 Main St, New York, USA");
  }

  [Fact]
  public void Equals_WithSameValues_ShouldReturnTrue()
  {
    // Arrange
    var locationResult1 = Location.Create("123 Main St", "New York", "USA", -73.9665, 40.7812);
    var locationResult2 = Location.Create("123 Main St", "New York", "USA", -73.9665, 40.7812);

    // Act & Assert
    locationResult1.IsSuccess.ShouldBeTrue();
    locationResult2.IsSuccess.ShouldBeTrue();
    locationResult1.Value.ShouldBe(locationResult2.Value);
  }

  [Fact]
  public void Equals_WithDifferentValues_ShouldReturnFalse()
  {
    // Arrange
    var locationResult1 = Location.Create("123 Main St", "New York", "USA", -73.9665, 40.7812);
    var locationResult2 = Location.Create("456 Elm St", "New York", "USA", -73.9665, 40.7812);
    var locationResult3 = Location.Create("123 Main St", "Chicago", "USA", -73.9665, 40.7812);
    var locationResult4 = Location.Create("123 Main St", "New York", "Canada", -73.9665, 40.7812);
    var locationResult5 = Location.Create("123 Main St", "New York", "USA", -74.0, 40.7812);
    var locationResult6 = Location.Create("123 Main St", "New York", "USA", -73.9665, 40.8);

    // Act & Assert
    locationResult1.IsSuccess.ShouldBeTrue();
    locationResult2.IsSuccess.ShouldBeTrue();
    locationResult3.IsSuccess.ShouldBeTrue();
    locationResult4.IsSuccess.ShouldBeTrue();
    locationResult5.IsSuccess.ShouldBeTrue();
    locationResult6.IsSuccess.ShouldBeTrue();
    locationResult1.Value.ShouldNotBe(locationResult2.Value);
    locationResult1.Value.ShouldNotBe(locationResult3.Value);
    locationResult1.Value.ShouldNotBe(locationResult4.Value);
    locationResult1.Value.ShouldNotBe(locationResult5.Value);
    locationResult1.Value.ShouldNotBe(locationResult6.Value);
  }
}