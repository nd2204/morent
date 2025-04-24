using System;
using Morent.Application.Interfaces;
using Morent.Core.ValueObjects;

namespace Morent.Infrastructure.Services;

public class PaymentService : IPaymentService
{
  public Task<string> ProcessPaymentAsync(Guid userId, Guid rentalId, Money amount, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> RefundPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}
