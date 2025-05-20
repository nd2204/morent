using System;

namespace Morent.Application.Features.Review;


public class UpdateReviewCommand : ICommand<Result>
{
  public required Guid UserId { get; set; }
  public required Guid ReviewId { get; set; }
  public required int Rating { get; set; }
  public required string Comment { get; set; } = null!;
}

