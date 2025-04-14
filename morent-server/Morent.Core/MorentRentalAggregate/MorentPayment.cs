namespace Morent.Core.MorentRentalAggregate;

public class MorentPayment
{
  public int Id { get; private set; }

  public int RentalId { get; private set; }
  public MorentRental Rental { get; private set; } = null!;

  public decimal Amount { get; private set; }
  public string PaymentMethod { get; private set; } = null!;
  public MorentPaymentStatus PaymentStatus { get; private set; }
  public DateTime? PaidAt { get; private set; }
}
