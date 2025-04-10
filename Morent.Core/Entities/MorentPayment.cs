namespace Morent.Core.Entities;

public class MorentPayment
{
  public int Id { get; set; }

  public int RentalId { get; set; }
  public MorentRental Rental { get; set; } = null!;

  public decimal Amount { get; set; }
  public string PaymentMethod { get; set; } = null!;
  public string PaymentStatus { get; set; } = "pending"; // or "paid", "failed"
  public DateTime? PaidAt { get; set; }
}
