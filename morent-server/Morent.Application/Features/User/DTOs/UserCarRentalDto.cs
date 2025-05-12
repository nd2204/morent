using System;

namespace Morent.Application.Features.User.DTOs;

public class UserCarsReviewDto
{
  public required RentalDto Rental { get;set; }
  public required CarDto Car { get; set; }
  public required string CarImageUrl { get; set; }
  public required int Rating { get; set; }
  public required string Comment { get; set; }
  public required bool IsReviewed { get; set; }
}
