using System;
using Morent.Core.MorentReviewAggregate;
using Morent.Core.MorentReviewAggregate.Events;
using Shouldly;
using Xunit;

namespace Morent.Tests.Unit.MorentReviewAggregate;

public class MorentReviewTests
{
    private readonly Guid _validUserId = Guid.NewGuid();
    private readonly Guid _validCarId = Guid.NewGuid();
    private const int ValidRating = 4;
    private const string ValidComment = "Great car, very comfortable!";

    [Fact]
    public void Create_WithValidInputs_ShouldCreateReview()
    {
        // Act
        var result = MorentReview.Create(_validUserId, _validCarId, ValidRating, ValidComment);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.UserId.ShouldBe(_validUserId);
        result.Value.CarId.ShouldBe(_validCarId);
        result.Value.Rating.ShouldBe(ValidRating);
        result.Value.Comment.ShouldBe(ValidComment);
        result.Value.CreatedAt.ShouldBeLessThan(DateTime.UtcNow);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Create_WithInvalidRating_ShouldReturnFailure(int invalidRating)
    {
        // Act
        var result = MorentReview.Create(_validUserId, _validCarId, invalidRating, ValidComment);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Create_WithEmptyUserId_ShouldReturnFailure()
    {
        // Act
        var result = MorentReview.Create(Guid.Empty, _validCarId, ValidRating, ValidComment);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Create_WithEmptyCarId_ShouldReturnFailure()
    {
        // Act
        var result = MorentReview.Create(_validUserId, Guid.Empty, ValidRating, ValidComment);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Create_ShouldRaiseReviewCreatedEvent()
    {
        // Act
        var result = MorentReview.Create(_validUserId, _validCarId, ValidRating, ValidComment);

        // Assert
        result.Value.DomainEvents.ShouldContain(e => e is ReviewCreatedEvent);
        var @event = result.Value.DomainEvents.First() as ReviewCreatedEvent;
        @event.ShouldNotBeNull();
        @event!.CarId.ShouldBe(_validCarId);
        @event.UserId.ShouldBe(_validUserId);
        @event.Rating.ShouldBe(ValidRating);
    }

    [Fact]
    public void UpdateReview_WithValidInputs_ShouldUpdateReview()
    {
        // Arrange
        var review = MorentReview.Create(_validUserId, _validCarId, ValidRating, ValidComment).Value;
        var newRating = 5;
        var newComment = "Updated comment";

        // Act
        var result = review.UpdateReview(newRating, newComment);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        review.Rating.ShouldBe(newRating);
        review.Comment.ShouldBe(newComment);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void UpdateReview_WithInvalidRating_ShouldReturnFailure(int invalidRating)
    {
        // Arrange
        var review = MorentReview.Create(_validUserId, _validCarId, ValidRating, ValidComment).Value;

        // Act
        var result = review.UpdateReview(invalidRating, ValidComment);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void UpdateRating_WithValidRating_ShouldUpdateRating()
    {
        // Arrange
        var review = MorentReview.Create(_validUserId, _validCarId, ValidRating, ValidComment).Value;
        var newRating = 5;

        // Act
        review.UpdateRating(newRating);

        // Assert
        review.Rating.ShouldBe(newRating);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void UpdateRating_WithInvalidRating_ShouldThrowException(int invalidRating)
    {
        // Arrange
        var review = MorentReview.Create(_validUserId, _validCarId, ValidRating, ValidComment).Value;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => review.UpdateRating(invalidRating));
    }

    [Fact]
    public void UpdateComment_ShouldUpdateComment()
    {
        // Arrange
        var review = MorentReview.Create(_validUserId, _validCarId, ValidRating, ValidComment).Value;
        var newComment = "Updated comment";

        // Act
        review.UpdateComment(newComment);

        // Assert
        review.Comment.ShouldBe(newComment);
    }
}