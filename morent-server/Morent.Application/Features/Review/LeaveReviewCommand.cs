using System;

namespace Morent.Application.Features.Review;

public class LeaveReviewCommand : ICommand<Result<ReviewDto>>
{
  public required Guid UserId { get; set; }
  public required Guid CarId { get; set; }
  public required LeaveReviewRequest Request { get; set; }
}