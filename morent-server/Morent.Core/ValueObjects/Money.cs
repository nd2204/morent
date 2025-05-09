using System;
using Morent.Core.Exceptions;

namespace Morent.Core.ValueObjects;

public class Money : ValueObject
{
  public decimal Amount { get; }
  public string Currency { get; }

  private Money() { }

  private Money(decimal amount, string currency = "USD")
  {
    Amount = amount;
    Currency = currency;
  }

  public static Result<Money> Create(decimal amount, string currency = "USD")
  {
    if (amount < 0)
      return Result.Invalid(new ValidationError(nameof(amount), "Amount cannot be negative"));

    if (string.IsNullOrWhiteSpace(currency))
      return Result.Invalid(new ValidationError(nameof(currency), "Currency cannot be empty"));

    return Result.Success(new Money(amount, currency));
  }

  public static Result<Money> Zero(string currency = "USD") => Money.Create(0, currency);

  public Result<Money> Add(Money other)
  {
    if (Currency != other.Currency)
      return Result.Invalid(new ValidationError($"Cannot add money with different currencies: {Currency} and {other.Currency}"));

    return Result.Success(new Money(Amount + other.Amount, Currency));
  }

  public Result<Money> Subtract(Money other)
  {
    if (Currency != other.Currency)
      return Result.Invalid(new ValidationError($"Cannot subtract money with different currencies: {Currency} and {other.Currency}"));

    var result = Amount - other.Amount;
    if (result < 0)
      return Result.Invalid(new ValidationError("Subtraction would result in negative amount."));

    return Result.Success(new Money(result, Currency));
  }

  public Result<Money> Multiply(decimal factor)
  {
    if (factor < 0)
      return Result.Invalid(new ValidationError("Cannot multiply by negative factor."));

    return Result.Success(new Money(Amount * factor, Currency));
  }

  public override string ToString() => $"{Amount} {Currency}";

  public override bool Equals(object? obj)
  {
    if (obj is Money other)
    {
      return Amount == other.Amount && Currency == other.Currency;
    }
    return false;
  }

  public override int GetHashCode() => HashCode.Combine(Amount, Currency);

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Amount;
    yield return Currency;
  }
}