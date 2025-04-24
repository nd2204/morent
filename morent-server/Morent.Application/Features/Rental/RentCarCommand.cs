using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Rental;

public class RentCarCommand : ICommand<Guid>
{
  public Guid UserId { get; set; }
  public Guid CarId { get; set; }
  public DateTime PickupDate { get; set; }
  public DateTime DropoffDate { get; set; }
  public CarLocationDto PickupLocation { get; set; } = null!;
  public CarLocationDto DropoffLocation { get; set; } = null!;
  public string PaymentMethodId { get; set; } = null!;
}
