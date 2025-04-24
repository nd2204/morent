using System;

namespace Morent.Application.Features.Review;


public class UpdateReviewCommand : ICommand<bool>
{
  public Guid ReviewId { get; set; }
  public int Rating { get; set; }
  public string Comment { get; set; } = null!;
}

