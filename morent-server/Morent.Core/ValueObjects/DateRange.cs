using System;

namespace Morent.Core.ValueObjects;

public class DateRange : ValueObject
{
  public DateTime Start { get; }
  public DateTime End { get; }
  public TimeSpan Duration => End - Start;

  private DateRange(DateTime start, DateTime end)
  {
    Start = start;
    End = end;
  }

  public static Result<DateRange> Create(DateTime start, DateTime end)
  {
    if (end < start)
    {
      return Result.Invalid(
        new ValidationError(nameof(DateRange), "End date cannot be before start date."));
    }

    return Result.Success(new DateRange(start, end));
  }

  public bool Overlaps(DateRange other)
  {
    return Start < other.End && other.Start < End;
  }

  public bool Contains(DateTime date)
  {
    return Start <= date && date <= End;
  }

  public int TotalDays()
  {
    return (int)Math.Ceiling(Duration.TotalDays);
  }

  public override string ToString() => $"{Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";

  public override bool Equals(object? obj)
  {
    if (obj is DateRange other)
    {
      return Start.Equals(other.Start) && End.Equals(other.End);
    }
    return false;
  }

  public override int GetHashCode() => HashCode.Combine(Start, End);

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Start;
    yield return End;
  }
}