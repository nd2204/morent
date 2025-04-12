using System.ComponentModel.DataAnnotations.Schema;

namespace Morent.Core.Entities;

public class MorentRentalDetail
{
  public int Id { get; set; }

  public int RentalId { get; set; }
  [ForeignKey(nameof(RentalId))]
  public MorentRental Rental { get; set; } = null!;

  public double PickupLat { get; set; }
  public double PickupLng { get; set; }

  public double DropoffLng { get; set; }
  public double DropoffLat { get; set; }

  public DateTime PickupDateTime { get; set; }
  public DateTime DropoffDateTime { get; set; }
}
