using System;

namespace Morent.Application.Interfaces;

public interface IPaymentProvider
{
  string ProviderId { get; }
  Task<Result<PaymentResponse>> CreatePaymentAsync(PaymentRequest request);
  Task<Result<PaymentResponse>> VerifyPaymentAsync(IDictionary<string, string> callbackData);
  Task<Result<PaymentResponse>> RefundPaymentAsync(string transactionId, decimal amount);
}