using System;

namespace Morent.UnitTests.Core.ValueObjects;

using Ardalis.Result;
using Shouldly;
using Morent.Core.ValueObjects;
using System;
using Xunit;

public class DateRangeTests
{
  [Fact]
  public void Constructor_WithValidDates_ShouldCreateDateRange()
  {
    // Arrange
    var startDate = DateTime.Today;
    var endDate = DateTime.Today.AddDays(7);

    // Act
    var dateRange = DateRange.Create(startDate, endDate);

    // Assert
    dateRange.Value.Start.ShouldBe(startDate);
    dateRange.Value.End.ShouldBe(endDate);
  }

  [Fact]
  public void Constructor_WithEndDateBeforeStartDate_ShouldReturnValidationError()
  {
    // Arrange
    var startDate = DateTime.Today;
    var endDate = DateTime.Today.AddDays(-1);

    // Act & Assert
    var dateRange = DateRange.Create(startDate, endDate);
    dateRange.IsInvalid().ShouldBeTrue();
    dateRange.ValidationErrors.Count().ShouldBeGreaterThan(0, "ValidationErrors should return a message");
  }

  [Fact]
  public void Constructor_WithEndDateEqualToStartDate_ShouldCreateDateRange()
  {
    // Arrange
    var date = DateTime.Today;

    // Act
    var dateRange = DateRange.Create(date, date);

    // Assert
    dateRange.Value.Start.ShouldBe(date);
    dateRange.Value.End.ShouldBe(date);
    dateRange.Value.Duration.ShouldBe(TimeSpan.FromDays(1), "Duration should always > 0");
  }

  [Fact]
  public void Duration_ShouldReturnCorrectNumberOfDays()
  {
    // Arrange
    var startDate = DateTime.Today;
    var endDate = DateTime.Today.AddDays(5);
    TimeSpan expectedDuration = endDate - startDate;

    // Act
    var dateRangeResult = DateRange.Create(startDate, endDate);

    // Assert
    dateRangeResult.IsSuccess.ShouldBeTrue();
    var duration = dateRangeResult.Value.Duration;
    duration.ShouldBe(expectedDuration);
  }

  [Fact]
  public void Overlaps_WithOverlappingRange_ShouldReturnTrue()
  {
    // Arrange
    var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));
    var range2 = DateRange.Create(new DateTime(2023, 1, 5), new DateTime(2023, 1, 15));

    // Act & Assert
    var overlaps = range1.Value.Overlaps(range2.Value);
    overlaps.ShouldBeTrue();
  }

  [Fact]
  public void Overlaps_WithAdjacentRanges_ShouldReturnFalse()
  {
    // Arrange
    var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));
    var range2 = DateRange.Create(new DateTime(2023, 1, 11), new DateTime(2023, 1, 15));

    // Act & Assert
    var overlaps = range1.Value.Overlaps(range2.Value);
    overlaps.ShouldBeFalse();
  }

  [Fact]
  public void Overlaps_WithNonOverlappingRanges_ShouldReturnFalse()
  {
    // Arrange
    var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));
    var range2 = DateRange.Create(new DateTime(2023, 1, 20), new DateTime(2023, 1, 25));

    // Act & Assert
    var overlaps = range1.Value.Overlaps(range2.Value);
    overlaps.ShouldBeFalse();
  }

  [Fact]
  public void Equals_WithSameValues_ShouldReturnTrue()
  {
    // Arrange
    var date1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));
    var date2 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));

    // Act & Assert
    date1.Value.ShouldBe(date2.Value);
  }

  [Fact]
  public void Equals_WithDifferentValues_ShouldReturnFalse()
  {
    // Arrange
    var date1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));
    var date2 = DateRange.Create(new DateTime(2023, 1, 2), new DateTime(2023, 1, 10));
    var date3 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 11));

    // Act & Assert
    date1.Value.ShouldNotBe(date2.Value);
    date1.Value.ShouldNotBe(date3.Value);
  }

  [Fact]
  public void Contains_WithDateInRange_ShouldReturnTrue()
  {
    // Arrange
    var dateRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));
    var date = new DateTime(2023, 1, 5);

    // Act
    var result = dateRange.Value.Contains(date);

    // Assert
    result.ShouldBeTrue();
  }

  [Fact]
  public void Contains_WithDateOutsideRange_ShouldReturnFalse()
  {
    // Arrange
    var dateRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 10));
    var date = new DateTime(2023, 1, 15);

    // Act
    var result = dateRange.Value.Contains(date);

    // Assert
    result.ShouldBeFalse();
  }

  [Fact]
  public void Contains_WithDateAtRangeBoundaries_ShouldReturnTrue()
  {
    // Arrange
    var startDate = new DateTime(2023, 1, 1);
    var endDate = new DateTime(2023, 1, 10);
    var dateRange = DateRange.Create(startDate, endDate);

    // Act & Assert
    dateRange.Value.Contains(startDate).ShouldBeTrue();
    dateRange.Value.Contains(endDate).ShouldBeTrue();
  }

}