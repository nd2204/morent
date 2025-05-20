using Morent.Core.ValueObjects;

namespace Morent.Application.Interfaces;

public interface IPaymentService
{
  Task<Result<IEnumerable<PaymentMethodDto>>> GetAllProviderDto();
  Task<Result<PaymentResponse>> ProcessPaymentAsync(PaymentRequest request);
  Task<Result<PaymentResponse>> VerifyPaymentAsync(string providerId, IDictionary<string, string> callbackData);
  Task<Result<PaymentResponse>> RefundPaymentAsync(string providerId, string transactionId, decimal amount);
}