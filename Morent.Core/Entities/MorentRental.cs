namespace Morent.Core.Entities;

public class MorentRental
{
  public int Id { get; set; }
  public string RentalStatus { get; set; } = null!;

  public int UserId { get; set; }
  public MorentUser User { get; set; } = null!;

  public int CarId { get; set; }
  public MorentCar Car { get; set; } = null!;

  public MorentRentalDetail RentalDetail { get; set; } = null!;
  public MorentPayment? Payment { get; set; }
}
