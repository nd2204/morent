namespace Morent.Core.MorentCarAggregate;

public class MorentOwnedCarStatus : SmartEnum<MorentOwnedCarStatus>
{
  public static readonly MorentOwnedCarStatus Available = new MorentOwnedCarStatus(nameof(Available), 0);
  public static readonly MorentOwnedCarStatus Rented = new MorentOwnedCarStatus(nameof(Rented), 1);
  public static readonly MorentOwnedCarStatus InMaintenance = new MorentOwnedCarStatus(nameof(InMaintenance), 2);

  private MorentOwnedCarStatus(string name, int value) : base(name, value) { }
}