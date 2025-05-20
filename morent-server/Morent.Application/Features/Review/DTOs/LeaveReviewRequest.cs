using System;

namespace Morent.Application.Features.Review.DTOs;

public class LeaveReviewRequest
{
  public required Guid RentalId { get; set; }
  public required int Rating { get; set; }
  public required string Comment { get; set; }
}
