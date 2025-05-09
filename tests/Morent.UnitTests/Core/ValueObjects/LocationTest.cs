namespace Morent.UnitTests.Core.ValueObjects;

using Morent.Core.ValueObjects;
using System;
using Xunit;
using Shouldly;
using Ardalis.Result;

public class LocationTests
{
  [Fact]
  public void Constructor_WithValidParameters_ShouldCreateLocation()
  {
    // Arrange & Act
    var locationResult = Location.Create("123 Main St", "New York", "USA");

    // Assert
    locationResult.IsSuccess.ShouldBeTrue();
    var location = locationResult.Value;
    location.Address.ShouldBe("123 Main St");
    location.City.ShouldBe("New York");
    location.Country.ShouldBe("USA");
  }

  [Theory]
  [InlineData(null, "New York", "USA")]
  [InlineData("", "New York", "USA")]
  [InlineData("123 Main St", null, "USA")]
  [InlineData("123 Main St", "", "USA")]
  [InlineData("123 Main St", "New York", null)]
  [InlineData("123 Main St", "New York", "")]
  public void Constructor_WithInvalidParameters_ShouldThrowException(string? address, string? city, string? country)
  {
    // Arrange & Act
#pragma warning disable CS8604 // Possible null reference argument.
    var locationResult = Location.Create(address, city, country);
#pragma warning restore CS8604 // Possible null reference argument.

    // Assert
    locationResult.Status.ShouldBe(ResultStatus.Invalid);
    locationResult.ValidationErrors.Count().ShouldBeGreaterThan(0);
  }

  [Fact]
  public void ToString_ShouldReturnFormattedString()
  {
    // Arrange
    var locationResult = Location.Create("123 Main St", "New York", "USA");

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
    var locationResult1 = Location.Create("123 Main St", "New York", "USA");
    var locationResult2 = Location.Create("123 Main St", "New York", "USA");

    // Act & Assert
    locationResult1.Value.ShouldBe(locationResult2.Value);
  }

  [Fact]
  public void Equals_WithDifferentValues_ShouldReturnFalse()
  {
    // Arrange
    var locationResult1 = Location.Create("123 Main St", "New York", "USA");
    var locationResult2 = Location.Create("456 Elm St", "New York", "USA");
    var locationResult3 = Location.Create("123 Main St", "Chicago", "USA");
    var locationResult4 = Location.Create("123 Main St", "New York", "Canada");

    // Act & Assert
    locationResult1.Value.ShouldNotBe(locationResult2.Value);
    locationResult1.Value.ShouldNotBe(locationResult3.Value);
    locationResult1.Value.ShouldNotBe(locationResult4.Value);
  }
}