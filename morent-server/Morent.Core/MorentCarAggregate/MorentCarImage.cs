using System;
using Morent.Core.Exceptions;
using Morent.Core.MediaAggregate;

namespace Morent.Core.MorentCarAggregate;

public class MorentCarImage : EntityBase<int>
{
  public Guid CarId { get; private set; }
  public Guid ImageId { get; private set; }
  public bool IsPrimary { get; private set; }
  public int DisplayOrder { get; private set; }

  public MorentCarImage(Guid carId, Guid imageId, bool isPrimary, int displayOrder)
  {
    CarId = carId;
    ImageId = imageId;
    IsPrimary = isPrimary;
    DisplayOrder = displayOrder;
  }

  // Methods
  public void SetAsPrimary(bool isPrimary)
  {
    IsPrimary = isPrimary;
  }

  public void UpdateDisplayOrder(int newOrder)
  {
    if (newOrder < 1)
      throw new DomainException("Display order must be at least 1");

    DisplayOrder = newOrder;
  }
}
