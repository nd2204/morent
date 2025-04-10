namespace Morent.Core.Entities;

public class MorentReview
{
  public int Id { get; set; }

  public int UserId { get; set; }
  public MorentUser User { get; set; } = null!;

  public int CarId { get; set; }
  public MorentCar Car { get; set; } = null!;

  public decimal Rating { get; set; }
  public string Comment { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
}
