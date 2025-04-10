namespace Morent.Core.Entities;

public class MorentLocation
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Address { get; set; } = null!;
  public string City { get; set; } = null!;
  public string State { get; set; } = null!;
  public string ZipCode { get; set; } = null!;
}
