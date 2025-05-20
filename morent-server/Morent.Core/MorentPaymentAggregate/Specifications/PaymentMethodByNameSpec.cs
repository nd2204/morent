using System;

namespace Morent.Core.MorentPaymentAggregate.Specifications;

public class PaymentMethodByNameSpec : Specification<PaymentProvider>
{
  public PaymentMethodByNameSpec(string name)
  {
    Query.Where(m => m.Name == name);
  }
}
