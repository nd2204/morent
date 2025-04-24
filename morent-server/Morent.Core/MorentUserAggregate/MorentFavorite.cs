using Morent.Core.MorentCarAggregate;
using Morent.Core.MorentUserAggregate;

namespace Morent.Core.MorentUserAggregate;

public class MorentFavorite : EntityBase
{
  private MorentFavorite() { }

  public MorentFavorite(Guid userId, int carId)
  {
    UserId = userId;
    CarId = carId;
  }

  public Guid UserId     { get; private set; }
  public MorentUser User { get; private set; } = null!;

  public int CarId     { get; private set; }
  public MorentCar Car { get; private set; } = null!;
}