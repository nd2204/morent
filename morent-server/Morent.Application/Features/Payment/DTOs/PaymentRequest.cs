using System;

namespace Morent.Application.Features.Payment.DTOs;

public class PaymentRequest
{
  public required Guid RentalId { get; set; }
  public required string Method { get; set; }
  public string Description { get; set; }
  public string ReturnUrl { get; set; }
  public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
  public required string PaymentProviderId { get; set; }
}
