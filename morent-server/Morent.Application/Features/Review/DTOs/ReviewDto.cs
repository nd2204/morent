namespace Morent.Application.Features.Review.DTOs;

public class ReviewDto
{
  public required Guid Id { get; set; }
  public required Guid UserId { get; set; }
  public required string UserName { get; set; } = null!;
  public required Guid CarId { get; set; }
  public required int Rating { get; set; }
  public required string Comment { get; set; } = null!;
  public required DateTime CreatedAt { get; set; }
  public required string UserImageUrl { get; set; } = default!; 
}