namespace Morent.Core.Entities;

public class MorentFavorite
{
  public Guid UserId { get; set; }
  public MorentUser User { get; set; } = null!;

  public int CarId { get; set; }
  public MorentCar Car { get; set; } = null!;
}