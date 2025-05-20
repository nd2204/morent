namespace Morent.Application.Features.Rental.Commands;

public class RentCarCommand : ICommand<Result<RentalDto>>
{
  public required Guid UserId { get; set; }
  public required Guid CarId { get; set; }
  public required DateTime PickupDate { get; set; }
  public required DateTime DropoffDate { get; set; }
  public required LocationDto PickupLocation { get; set; } = null!;
  public required LocationDto DropoffLocation { get; set; } = null!;
}
