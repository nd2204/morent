using System;

namespace Morent.Application.Models;

public class RefreshTokenDetails
{
  public string Token { get; set; } = null!;
  public DateTime Created { get; set; }
  public DateTime Expired { get; set; }
}
