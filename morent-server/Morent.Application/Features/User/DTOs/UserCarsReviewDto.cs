using System;

namespace Morent.Application.Features.User.DTOs;

public class UserCarsReviewDto
{
  public Guid RentalId;
  public string CarImageUrl { get; set; }
  public int Rating;
  public bool IsReviewed;
}
