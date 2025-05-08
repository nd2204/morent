namespace Morent.Application.Features.Review.DTOs;

public record class UpdateReviewRequest
{
  public int Rating { get; set; }
  public string Comment { get; set; }
}
