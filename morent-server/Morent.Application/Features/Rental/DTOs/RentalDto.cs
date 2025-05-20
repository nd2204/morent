namespace Morent.Application.Features.Rental.DTOs;

public class RentalDto
{
  public required Guid Id { get; set; }
  public required Guid UserId { get; set; }
  public required Guid CarId { get; set; }
  public required DateTime PickupDate { get; set; }
  public required DateTime DropoffDate { get; set; }
  public required LocationDto PickupLocation { get; set; } = null!;
  public required LocationDto DropoffLocation { get; set; } = null!;
  public required decimal TotalCost { get; set; }
  public required string Currency { get; set; } = null!;
  public required string Status { get; set; }
  public required DateTime CreatedAt { get; set; }
}