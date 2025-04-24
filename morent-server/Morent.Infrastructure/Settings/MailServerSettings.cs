using System;

namespace Morent.Infrastructure.Settings;

public class MailServerSettings
{ 
  public string Hostname { get; set; } = "localhost";
  public int Port { get; set; } = 25;
}
