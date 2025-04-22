using System;

namespace Morent.Application.DTOs;

public class ReviewDto
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string UserName { get; set; } = null!;
  public Guid CarId { get; set; }
  public string CarDetails { get; set; } = null!;
  public int Rating { get; set; }
  public string Comment { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
}