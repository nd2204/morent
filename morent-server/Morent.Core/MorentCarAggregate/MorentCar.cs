namespace Morent.Core.MorentCarAggregate;

public class MorentCar : EntityBase, IAggregateRoot
{
  public string Make { get; set; } = null!; // Or brand
  public string Model { get; set; } = null!;
  public int Capacity { get; set; }
  public int Year { get; set; }
  public int FuelCapacityLitter { get; set; }
  public MorentCarTransmissionType Transmission { get; set; } = null!;
  public decimal PricePerDay { get; set; }
  public MorentCarBodyType BodyType { get; set; } = null!;

  private readonly List<MorentOwnedCar> _ownedCars = new();
  private readonly List<MorentImage> _images = new();

  public IEnumerable<MorentOwnedCar> OwnedCars => _ownedCars.AsReadOnly();
  public IEnumerable<MorentImage> Images => _images.AsReadOnly();

  public void AddCar(MorentOwnedCar car)
  {
    Guard.Against.Null(car);
    _ownedCars.Add(car);
  }

  public void AddImage(MorentImage image)
  {
    Guard.Against.Null(image);
    _images.Add(image);
  }
}
