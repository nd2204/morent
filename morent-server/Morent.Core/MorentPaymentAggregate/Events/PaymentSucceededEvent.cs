using System;

namespace Morent.Core.MorentPaymentAggregate;

public class PaymentSucceededEvent : DomainEventBase
{
  public Guid PaymentId { get; }

  public PaymentSucceededEvent(Guid paymentId) {
    PaymentId = paymentId;
  }
}
