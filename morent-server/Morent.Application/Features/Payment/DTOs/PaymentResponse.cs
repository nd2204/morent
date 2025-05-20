using System;

namespace Morent.Application.Features.Payment.DTOs;

public class PaymentResponse
{
  public string TransactionId { get; set; }
  public required PaymentStatus Status { get; set; }
  public string PaymentUrl { get; set; }
  public string Message { get; set; }
  public Dictionary<string, string> ProviderData { get; set; } = new Dictionary<string, string>();
}
