using System;

namespace Morent.Application.Features.Review;

public class LeaveReviewCommand : ICommand<Guid>
{
  public Guid CarId { get; set; }
  public Guid RentalId { get; set; }
  public int Rating { get; set; }
  public string Comment { get; set; } = null!;
}