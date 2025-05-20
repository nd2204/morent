using System;
using Morent.Application.Features.Payment.DTOs;
using Morent.Core.MorentPaymentAggregate;

namespace Morent.Infrastructure.Payment;

public class MoMoPaymentProvider : IPaymentProvider
{
  public static string Id = "momo";
  public string ProviderId => Id;

  public Task<Result<PaymentResponse>> CreatePaymentAsync(PaymentRequest request)
  {
    throw new NotImplementedException();
  }

  public Task<Result<PaymentResponse>> RefundPaymentAsync(string transactionId, decimal amount)
  {
    throw new NotImplementedException();
  }

  public Task<Result<PaymentResponse>> VerifyPaymentAsync(IDictionary<string, string> callbackData)
  {
    throw new NotImplementedException();
  }
}
