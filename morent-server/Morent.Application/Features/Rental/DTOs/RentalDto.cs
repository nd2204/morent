namespace Morent.Application.Features.Rental.DTOs;

public class RentalDto
{
  public Guid Id { get; set; }
  public CarDto CarInfo { get; set; } = null!;
  public DateTime PickupDate { get; set; }
  public DateTime DropoffDate { get; set; }
  public CarLocationDto PickupLocation { get; set; } = null!;
  public CarLocationDto DropoffLocation { get; set; } = null!;
  public decimal TotalCost { get; set; }
  public string Currency { get; set; } = null!;
  public MorentRentalStatus Status { get; set; }
  public DateTime CreatedAt { get; set; }
}