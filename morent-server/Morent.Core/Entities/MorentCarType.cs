namespace Morent.Core.Entities;

public class MorentCarType
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;

  public ICollection<MorentCarModel> CarModels { get; set; } = new List<MorentCarModel>();
}
