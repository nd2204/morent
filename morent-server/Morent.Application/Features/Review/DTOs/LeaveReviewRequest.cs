using System;

namespace Morent.Application.Features.Review.DTOs;

public class LeaveReviewRequest
{
  public Guid RentalId { get; set; }
  public int Rating { get; set; }
  public string Comment { get; set; }
}
