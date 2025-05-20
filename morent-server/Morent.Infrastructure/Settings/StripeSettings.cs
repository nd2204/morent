using System;

namespace Morent.Infrastructure.Settings;

public class StripeSettings
{
  public string SecretKey { get; set; }
  public string PublishableKey { get; set; }
}
