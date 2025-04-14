namespace Morent.Core.MorentCarAggregate;

public class MorentCarTransmissionType : SmartEnum<MorentCarTransmissionType>
{
  public static readonly MorentCarTransmissionType Manual = new("Manual", 1);
  public static readonly MorentCarTransmissionType Automatic = new("Automatic", 2);

  private MorentCarTransmissionType(string name, int value) : base(name, value) { }
}