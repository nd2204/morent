using System;

namespace Morent.Core.MorentUserAggregate;

public class MorentUserOAuthLogin : EntityBase
{
  public string Provider { get; private set; }
  public string ProviderKey { get; private set; }

  private MorentUserOAuthLogin() { } // For EF Core

  public MorentUserOAuthLogin(string provider, string providerKey)
  {
    Provider = provider;
    ProviderKey = providerKey;
  }
}
