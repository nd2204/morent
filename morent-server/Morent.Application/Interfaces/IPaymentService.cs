using Morent.Core.ValueObjects;

namespace Morent.Application.Interfaces;

public interface IPaymentService
{
  Task<string> ProcessPaymentAsync(Guid userId, Guid rentalId, Money amount, CancellationToken cancellationToken = default);
  Task<bool> RefundPaymentAsync(string paymentId, CancellationToken cancellationToken = default);
}