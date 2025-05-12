using System;

namespace Morent.Application.Features.Rental.DTOs;

public class RentalDetailDto
{
  public required DateTime PickupDate { get; set; }
  public required DateTime DropoffDate { get; set; }
  public required CarLocationDto PickupLocation { get; set; } = null!;
  public required CarLocationDto DropoffLocation { get; set; } = null!;
}
