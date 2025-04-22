using System;
using Morent.Core.Exceptions;

namespace Morent.Core.ValueObjects;

public class Money : ValueObject
{
  public decimal Amount { get; }
  public string Currency { get; }

  private Money() { }

  public Money(decimal amount, string currency = "USD")
  {
    if (amount < 0)
      throw new ArgumentException("Amount cannot be negative", nameof(amount));

    if (string.IsNullOrWhiteSpace(currency))
      throw new ArgumentException("Currency cannot be empty", nameof(currency));

    Amount = amount;
    Currency = currency;
  }

  public static Money Zero(string currency = "USD") => new Money(0, currency);

  public Money Add(Money other)
  {
    if (Currency != other.Currency)
      throw new DomainException($"Cannot add money with different currencies: {Currency} and {other.Currency}");

    return new Money(Amount + other.Amount, Currency);
  }

  public Money Subtract(Money other)
  {
    if (Currency != other.Currency)
      throw new DomainException($"Cannot subtract money with different currencies: {Currency} and {other.Currency}");

    var result = Amount - other.Amount;
    if (result < 0)
    {
      throw new DomainException("Subtraction would result in negative amount.");
    }

    return new Money(result, Currency);
  }

  public Money Multiply(decimal factor)
  {
    if (factor < 0)
      throw new DomainException("Cannot multiply by negative factor.");

    return new Money(Amount * factor, Currency);
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