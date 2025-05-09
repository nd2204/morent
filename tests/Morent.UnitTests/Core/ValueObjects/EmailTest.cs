using Shouldly;
using Morent.Core.ValueObjects;
using Ardalis.Result;
using Xunit;

namespace Morent.UnitTests.Core.ValueObjects;

public class EmailTests
{
  [Fact]
  public void Constructor_WithValidEmail_ShouldCreateEmail()
  {
    // Arrange & Act
    var emailResult = Email.Create("test@example.com");

    // Assert
    emailResult.IsSuccess.ShouldBeTrue();
    emailResult.Value.ShouldNotBeNull();
    emailResult.Value.ToString().ShouldBe("test@example.com");
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_WithEmptyEmail_ShouldBeInvalid(string? email)
  {
    // Arrange & Act
#pragma warning disable CS8604 // Possible null reference argument.
    var emailResult = Email.Create(email);
#pragma warning restore CS8604 // Possible null reference argument.

    // Assert
    emailResult.IsSuccess.ShouldBeFalse();
    emailResult.Status.ShouldBe(ResultStatus.Invalid);
    emailResult.ValidationErrors.Count().ShouldBeGreaterThan(0);
  }

  [Theory]
  [InlineData("notanemail")]
  [InlineData("missing@tld")]
  [InlineData("@missingusername.com")]
  [InlineData("test@.com")]
  public void Constructor_WithInvalidEmailFormat_ShouldBeInvalid(string email)
  {
    // Arrange & Act
    var emailResult = Email.Create(email);

    // Assert
    emailResult.IsSuccess.ShouldBeFalse();
    emailResult.Status.ShouldBe(ResultStatus.Invalid);
    emailResult.ValidationErrors.Count().ShouldBeGreaterThan(0);
  }

  [Fact]
  public void Equals_WithSameValue_ShouldReturnTrue()
  {
    // Arrange
    var email1Result = Email.Create("test@example.com");
    var email2Result = Email.Create("test@example.com");

    // Act & Assert
    email1Result.IsSuccess.ShouldBeTrue();
    email2Result.IsSuccess.ShouldBeTrue();
    
    email1Result.Value.Equals(email2Result.Value).ShouldBeTrue();
  }

  [Fact]
  public void Equals_WithDifferentCase_ShouldReturnTrue()
  {
    // Arrange
    var email1Result = Email.Create("Test@Example.com");
    var email2Result = Email.Create("test@example.com");

    // Act & Assert
    email1Result.IsSuccess.ShouldBeTrue();
    email2Result.IsSuccess.ShouldBeTrue();
    
    email1Result.Value.Equals(email2Result.Value).ShouldBeTrue();
  }

  [Fact]
  public void Equals_WithDifferentValue_ShouldReturnFalse()
  {
    // Arrange
    var email1Result = Email.Create("test1@example.com");
    var email2Result = Email.Create("test2@example.com");

    // Act & Assert
    email1Result.IsSuccess.ShouldBeTrue();
    email2Result.IsSuccess.ShouldBeTrue();
    
    email1Result.Value.Equals(email2Result.Value).ShouldBeFalse();
  }

  [Fact]
  public void ToString_ShouldReturnEmailValue()
  {
    // Arrange
    var emailResult = Email.Create("test@example.com");

    // Act & Assert
    emailResult.IsSuccess.ShouldBeTrue();
    emailResult.Value.ToString().ShouldBe("test@example.com");
  }
} 