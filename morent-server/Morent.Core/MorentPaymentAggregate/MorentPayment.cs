using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Morent.Core.MorentPaymentAggregate;

public class MorentPayment : EntityBase<Guid>, IAggregateRoot
{
  public Guid RentalId { get; private set; }
  public Money PaymentAmount { get; private set; }
  public PaymentMethod Method { get; private set; }
  public PaymentStatus Status { get; private set; }
  public string TransactionId { get; private set; }
  public DateTime? PaidAt { get; private set; }

  private MorentPayment() { }

  public MorentPayment(Guid rentalId, Money money, PaymentMethod method, PaymentStatus status, string transactionId)
  {
    if (money.Amount < 0) throw new ArgumentException("Money must be positive");

    RentalId = rentalId;
    PaymentAmount = money;
    Method = method;
    Status = status;
    TransactionId = transactionId;
  }

  public MorentPayment MarkAsSucceeded()
  {
    PaidAt = DateTime.UtcNow;
    Status = PaymentStatus.Succeeded;
    return this;
  }

  public MorentPayment MarkAsFailed()
  {
    PaidAt = null;
    Status = PaymentStatus.Failed;
    return this;
  }
}
