namespace Morent.Core.Entities;

public class MorentRentalDetail
{
  public int Id { get; set; }

  public int RentalId { get; set; }
  public MorentRental Rental { get; set; } = null!;

  public int PickupLocationId { get; set; }
  public MorentLocation PickupLocation { get; set; } = null!;

  public int DropoffLocationId { get; set; }
  public MorentLocation DropoffLocation { get; set; } = null!;

  public DateTime PickupDateTime { get; set; }
  public DateTime DropoffDateTime { get; set; }
}
