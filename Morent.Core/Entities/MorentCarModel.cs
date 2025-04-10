namespace Morent.Core.Entities;

public class MorentCarModel
{
  public int Id { get; set; }
  public int Capacity { get; set; }
  public string Brand { get; set; } = null!;
  public string Model { get; set; } = string.Empty;
  public int FuelCapacityLitter { get; set; }
  public string SteeringType { get; set; } = null!;

  public decimal PricePerDay { get; set; }

  public int CarTypeId { get; set; }
  public MorentCarType CarType { get; set; } = null!;

  public ICollection<MorentImage> Image { get; set; } = null!;
  public ICollection<MorentCar> Cars { get; set; } = new List<MorentCar>();
}
