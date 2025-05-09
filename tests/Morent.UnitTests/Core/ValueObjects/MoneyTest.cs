using Shouldly;
using Morent.Core.ValueObjects;
using Ardalis.Result;

namespace Morent.UnitTests.Core.ValueObjects;

public class MoneyTests
{
  [Fact]
  public void Constructor_WithValidParameters_ShouldCreateMoney()
  {
    // Arrange & Act
    var moneyResult = Money.Create(100.50m, "USD");

    // Assert
    moneyResult.IsSuccess.ShouldBeTrue();
    moneyResult.Value.ShouldNotBeNull();
    moneyResult.Value.Amount.ShouldBe(100.50m);
    moneyResult.Value.Currency.ShouldBe("USD");
  }

  [Fact]
  public void Constructor_WithNegativeAmount_ShouldBeInvalid()
  {
    // Arrange & Act & Assert
    var moneyResult = Money.Create(-100m);

    moneyResult.IsSuccess.ShouldBeFalse();
    moneyResult.Status.ShouldBe(ResultStatus.Invalid);
    moneyResult.ValidationErrors.Count().ShouldBeGreaterThan(
      0, "Result should return validation errors messages");
  }

  [Fact]
  public void Constructor_WithEmptyCurrency_ShouldBeInvalid()
  {
    // Arrange & Act & Assert
    Result<Money> moneyResult = Money.Create(100m, "");

    moneyResult.IsSuccess.ShouldBeFalse();
    moneyResult.Status.ShouldBe(ResultStatus.Invalid);
    moneyResult.ValidationErrors.Count().ShouldBeGreaterThan(
      0, "Result should return validation error messages");
  }

  [Fact]
  public void Add_WithSameCurrency_ShouldAddAmounts()
  {
    // Arrange
    var moneyResult1 = Money.Create(100m, "USD");
    var moneyResult2 = Money.Create(50m, "USD");

    // Act
    var result = moneyResult1.Value.Add(moneyResult2.Value);

    // Assert
    result.IsSuccess.ShouldBeTrue();
    result.Value.Amount.ShouldBe(150m);
    result.Value.Currency.ShouldBe("USD");
  }

  [Fact]
  public void Add_WithDifferentCurrencies_ShouldBeInvalid()
  {
    // Arrange
    var moneyResult1 = Money.Create(100m, "USD");
    var moneyResult2 = Money.Create(50m, "EUR");

    // Act & Assert
    var result = moneyResult1.Value.Add(moneyResult2.Value);

    result.Status.ShouldBe(ResultStatus.Invalid);
    result.ValidationErrors.Count().ShouldBeGreaterThan(
      0, "Result should return validation error messages");
  }

  [Fact]
  public void Subtract_WithSameCurrency_ShouldSubtractAmounts()
  {
    // Arrange
    var moneyResult1 = Money.Create(100m, "USD");
    var moneyResult2 = Money.Create(50m, "USD");

    // Act
    var result = moneyResult1.Value.Subtract(moneyResult2.Value);

    // Assert
    result.IsSuccess.ShouldBeTrue();
    result.Value.Amount.ShouldBe(50m);
    result.Value.Currency.ShouldBe("USD");
  }

  [Fact]
  public void Subtract_WithDifferentCurrencies_ShouldReturnValidationError()
  {
    // Arrange
    var moneyResult1 = Money.Create(100m, "USD");
    var moneyResult2 = Money.Create(50m, "EUR");

    // Act & Assert
    var result = moneyResult1.Value.Subtract(moneyResult2.Value);

    result.Status.ShouldBe(ResultStatus.Invalid);
    result.ValidationErrors.Count().ShouldBeGreaterThan(
      0, "Result should return validation error messages");
  }

  [Fact]
  public void Subtract_ResultingInNegativeAmount_ShouldBeInvalid()
  {
    // Arrange
    var moneyResult1 = Money.Create(50m, "USD");
    var moneyResult2 = Money.Create(100m, "USD");

    // Act & Assert
    var result = moneyResult1.Value.Subtract(moneyResult2.Value);

    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.Invalid);
    result.ValidationErrors.Count().ShouldBeGreaterThan(
      0, "Result should return validation error messages");
  }

  [Fact]
  public void Multiply_WithPositiveFactor_ShouldMultiplyAmount()
  {
    // Arrange
    var money = Money.Create(100m, "USD");

    // Act
    var result = money.Value.Multiply(2.5m);

    // Assert
    result.IsSuccess.ShouldBeTrue();
    result.Value.Amount.ShouldBe(250m);
    result.Value.Currency.ShouldBe("USD");
  }

  [Fact]
  public void Multiply_WithNegativeFactor_ShouldThrowDomainException()
  {
    // Arrange
    var money = Money.Create(100m, "USD");

    // Act & Assert
    var result = money.Value.Multiply(-2);
    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.Invalid);
    result.ValidationErrors.Count().ShouldBeGreaterThan(
      0, "Result should return validation error messages");
  }

  [Fact]
  public void Zero_ShouldCreateMoneyWithZeroAmount()
  {
    // Act
    var moneyResult = Money.Zero("EUR");

    // Assert
    moneyResult.IsSuccess.ShouldBeTrue();
    moneyResult.Value.Amount.ShouldBe(0m);
    moneyResult.Value.Currency.ShouldBe("EUR");
  }

  [Fact]
  public void Equals_WithSameValues_ShouldReturnTrue()
  {
    // Arrange
    var moneyResult1 = Money.Create(100m, "USD");
    var moneyResult2 = Money.Create(100m, "USD");

    // Act & Assert
    moneyResult1.Value.ShouldBe(moneyResult2.Value);
  }

  [Fact]
  public void Equals_WithDifferentValues_ShouldReturnFalse()
  {
    // Arrange
    var moneyResult1 = Money.Create(100m, "USD");
    var moneyResult2 = Money.Create(200m, "USD");
    var moneyResult3 = Money.Create(100m, "EUR");

    // Act & Assert
    moneyResult1.Value.ShouldNotBe(moneyResult2.Value);
    moneyResult1.Value.ShouldNotBe(moneyResult3.Value);
  }
}