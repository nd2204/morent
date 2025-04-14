using Morent.Core.MorentUserAggregate;

namespace Morent.WebApi.Controllers.MorentAuth.Signup;

public class SignupResponse
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string Role { get; set; } = null!;
}