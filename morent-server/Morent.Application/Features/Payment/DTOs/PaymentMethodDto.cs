using System;

namespace Morent.Application.Features.Payment.DTOs;

public class PaymentMethodDto
{
  public required string Id { get; set; }
  public required string Name { get; set; }
  public required string LogoUrl { get; set;  } 
  public required decimal FeePercent { get; set; }
}
