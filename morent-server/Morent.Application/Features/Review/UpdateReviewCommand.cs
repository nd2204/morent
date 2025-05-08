using System;

namespace Morent.Application.Features.Review;


public class UpdateReviewCommand : ICommand<Result>
{
  public Guid UserId { get; set; }
  public Guid ReviewId { get; set; }
  public int Rating { get; set; }
  public string Comment { get; set; } = null!;
}

