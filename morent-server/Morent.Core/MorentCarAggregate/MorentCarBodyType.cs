namespace Morent.Core.MorentCarAggregate;

public class MorentCarBodyType : SmartEnum<MorentCarBodyType>
{
  public static readonly MorentCarBodyType Sedan = new("Sedan", 0);
  public static readonly MorentCarBodyType Suv = new("Suv", 1);
  public static readonly MorentCarBodyType Hatchback = new("Hatchback", 2);
  public static readonly MorentCarBodyType HPV = new("HPV", 3);
  public static readonly MorentCarBodyType Sport = new("Sport", 4);
  public static readonly MorentCarBodyType Coupe = new("Coupe", 5);

  private MorentCarBodyType(string name, int value) : base(name, value) { }
}
