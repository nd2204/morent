namespace Morent.Core.MorentUserAggregate;

public class MorentUserRole : SmartEnum<MorentUserRole>
{
  public static readonly MorentUserRole Admin = new MorentUserRole(nameof(Admin), 0);
  public static readonly MorentUserRole User = new MorentUserRole(nameof(User), 1);

  protected MorentUserRole(string name, int value) : base(name, value) { }
}
