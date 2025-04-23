using Morent.Core.Exceptions;

namespace Morent.Core.MorentCarAggregate;

public class MorentCar : EntityBase<Guid>, IAggregateRoot
{
  public string Brand { get; private set; }
  public string Model { get; private set; }
  public int Year { get; private set; }
  public string LicensePlate { get; private set; }
  public int Capacity { get; private set; }
  public FuelType FuelType { get; private set; }
  public Money PricePerDay { get; private set; }
  public bool IsAvailable { get; private set; }
  public string? Description { get; private set; }
  public Location CurrentLocation { get; private set; }

  private readonly List<string> _images = new();
  private readonly List<MorentRental> _rentals = new List<MorentRental>();
  private readonly List<MorentReview> _reviews = new List<MorentReview>();

  public IReadOnlyCollection<string> Images => _images.AsReadOnly();
  public IReadOnlyCollection<MorentRental> Rentals => _rentals.AsReadOnly();
  public IReadOnlyCollection<MorentReview> Reviews => _reviews.AsReadOnly();

  private MorentCar() { }

  public MorentCar(string brand, string model, int year, string licensePlate, int capacity,
             FuelType fuelType, Money pricePerDay, Location currentLocation)
  {
    Guard.Against.NullOrEmpty(brand, nameof(brand));
    Guard.Against.NullOrEmpty(model, nameof(model));
    Guard.Against.NullOrEmpty(licensePlate, nameof(licensePlate));
    Id = Guid.NewGuid();
    Brand = brand;
    Model = model;
    Year = year;
    LicensePlate = licensePlate;
    Capacity = capacity;
    FuelType = fuelType;
    PricePerDay = pricePerDay;
    CurrentLocation = currentLocation;
    IsAvailable = true;
  }

  public static Result<MorentCar> Create(string brand, string model, int year, string licensePlate,
                                  int capacity, FuelType fuelType, Money pricePerDay, Location currentLocation)
  {
    var validateResult = ValidateCar(brand, model, year, licensePlate, capacity);
    if (validateResult.IsInvalid())
    {
      return Result.Invalid(validateResult.ValidationErrors);
    }

    var car = new MorentCar(brand, model, year, licensePlate, capacity, fuelType, pricePerDay, currentLocation);
    car.RegisterDomainEvent(new CarCreatedEvent(car.Id));

    return Result.Success(car);
  }

  public void AddRental(MorentRental rental)
  {
    Guard.Against.Null(rental, nameof(rental));
    if (!IsAvailableDuring(rental.RentalPeriod))
      throw new DomainException($"Car {Id} is not available during requested period");

    _rentals.Add(rental);
  }

  public void AddReview(MorentReview review)
  {
    Guard.Against.Null(review, nameof(review));
    _reviews.Add(review);
  }

  public void UpdateLocation(Location location)
  {
    Guard.Against.Null(location, nameof(location));
    CurrentLocation = location;
  }

  public Result UpdateDetails(string brand, string model, int year, string licensePlate,
                            int capacity, FuelType fuelType, Money pricePerDay, string? description)
  {
    var validateResult = ValidateCar(brand, model, year, licensePlate, capacity);
    if (validateResult.IsInvalid())
    {
      return Result.Invalid(validateResult.ValidationErrors);
    }

    Brand = brand;
    Model = model;
    Year = year;
    LicensePlate = licensePlate;
    Capacity = capacity;
    FuelType = fuelType;
    PricePerDay = pricePerDay;
    Description = description;

    // UpdateModifiedDate();

    return Result.Success();
  }

  public void AddImage(string imageUrl)
  {
    if (!string.IsNullOrWhiteSpace(imageUrl) && !_images.Contains(imageUrl))
    {
      _images.Add(imageUrl);
      // UpdateModifiedDate();
    }
  }

  public void RemoveImage(string imageUrl)
  {
    if (_images.Contains(imageUrl))
    {
      _images.Remove(imageUrl);
      // UpdateModifiedDate();
    }
  }

  public void SetAvailability(bool isAvailable)
  {
    IsAvailable = isAvailable;
    // UpdateModifiedDate();
  }

  public bool IsAvailableDuring(DateRange dateRange)
  {
    if (!IsAvailable)
      return false;

    foreach (var rental in _rentals)
    {
      if (rental.Status != MorentRentalStatus.Cancelled && rental.RentalPeriod.Overlaps(dateRange))
        return false;
    }

    return true;
  }

  private static Result ValidateCar(string brand, string model, int year, string licensePlate, int capacity)
  {
    if (string.IsNullOrWhiteSpace(brand))
      return Result.Invalid(new ValidationError("Car", "Brand cannot be empty."));

    if (string.IsNullOrWhiteSpace(model))
      return Result.Invalid(new ValidationError("Car", "Model cannot be empty."));

    if (year < 1900 || year > DateTime.UtcNow.Year + 1)
      return Result.Invalid(new ValidationError("Car", $"Year must be between 1900 and {DateTime.UtcNow.Year + 1}."));

    if (string.IsNullOrWhiteSpace(licensePlate))
      return Result.Invalid(new ValidationError("Car", "License plate cannot be empty."));

    if (capacity <= 0)
      return Result.Invalid(new ValidationError("Car", "Capacity must be greater than zero."));

    return Result.Success();
  }
}
