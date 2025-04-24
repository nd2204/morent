using System;

namespace Morent.Application.Features.Car.DTOs;

public class CarReviewDto
{
  public Guid UserId { get; set; }
  public string UserName { get; set; } = default!; // The name of the user (not the loginId)
  public int Rating { get; set; }
  public string Comment { get; set; } = default!;
  public string ImageUrl { get; set; } = default!; 
}