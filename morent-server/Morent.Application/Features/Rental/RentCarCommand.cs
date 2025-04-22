namespace Morent.Application.Features.Rental;

public class RentCarCommand : ICommand<Guid>
{
  public Guid CarId { get; set; }
  public DateTime PickupDate { get; set; }
  public DateTime DropoffDate { get; set; }
  public LocationDto PickupLocation { get; set; } = null!;
  public LocationDto DropoffLocation { get; set; } = null!;
  public string PaymentMethodId { get; set; } = null!;
}
