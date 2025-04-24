using System;

namespace Morent.Application.DTOs;

public class RentalDto
{
  public Guid Id { get; set; }
  public Guid CarId { get; set; }
  public string CarBrand { get; set; } = null!;
  public string CarModel { get; set; } = null!;
  public DateTime PickupDate { get; set; }
  public DateTime DropoffDate { get; set; }
  public LocationDto PickupLocation { get; set; } = null!;
  public LocationDto DropoffLocation { get; set; } = null!;
  public decimal TotalCost { get; set; }
  public string Currency { get; set; } = null!;
  public MorentRentalStatus Status { get; set; }
  public DateTime CreatedAt { get; set; }
}