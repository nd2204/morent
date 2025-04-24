using System.ComponentModel;
using Morent.Core.Exceptions;
using Morent.Core.MediaAggregate;

namespace Morent.Core.MorentCarAggregate;

public class MorentCar : EntityBase<Guid>, IAggregateRoot
{
  public Guid CarModelId { get; private set; }
  public MorentCarModel? CarModel { get; private set; } = null!;
  public string LicensePlate { get; private set; }
  public Money PricePerDay { get; private set; }
  public bool IsAvailable { get; private set; }
  public string? Description { get; private set; }
  public Location CurrentLocation { get; private set; }

  private readonly List<MorentCarImage> _images = new();
  private readonly List<MorentRental> _rentals = new List<MorentRental>();
  private readonly List<MorentReview> _reviews = new List<MorentReview>();

  public IReadOnlyCollection<MorentCarImage> Images => _images.AsReadOnly();
  public IReadOnlyCollection<MorentRental> Rentals => _rentals.AsReadOnly();
  public IReadOnlyCollection<MorentReview> Reviews => _reviews.AsReadOnly();

  private MorentCar() { }

  public MorentCar(Guid modelId, string licensePlate, Money pricePerDay, Location currentLocation, string? description = null)
  {
    Guard.Against.NullOrEmpty(licensePlate, nameof(licensePlate));
    Guard.Against.NullOrEmpty(modelId, nameof(modelId));

    Id = Guid.NewGuid();
    Description = description;
    CarModelId = modelId;
    LicensePlate = licensePlate;
    PricePerDay = pricePerDay;
    CurrentLocation = currentLocation;
    IsAvailable = true;
  }

  public void AddDescription(string description)
  {
    Description = description;
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

  public Result UpdatePrice(Money price)
  {
    Guard.Against.Null(price);
    PricePerDay = price;
    return Result.Success();
  }

  public Result UpdateDetails(string? licensePlate, Money? pricePerDay, string? description)
  {
    if (licensePlate != null)
      LicensePlate = licensePlate;

    if (pricePerDay is not null)
      PricePerDay = pricePerDay;

    if (description != null)
      Description = description;

    return Result.Success();
  }

  // Methods for managing images
  public MorentCarImage AddImage(MorentImage image, bool isPrimary = false)
  {
    // Business rule: Maximum 10 images per car
    if (_images.Count >= 10)
      throw new DomainException("A car cannot have more than 10 images");

    // If this is the first image or isPrimary is true, ensure this is the primary image
    if (isPrimary || !_images.Any())
    {
      // Set all existing images as non-primary
      foreach (var existingImage in _images.Where(i => i.IsPrimary))
      {
        existingImage.SetAsPrimary(false);
      }

      var nextDisplayOrder = _images.Any() ? _images.Max(i => i.DisplayOrder) + 1 : 1;
      var carImage = new MorentCarImage(this.Id, image.Id, isPrimary: true, displayOrder: nextDisplayOrder);
      _images.Add(carImage);
      return carImage;
    }
    else
    {
      var nextDisplayOrder = _images.Any() ? _images.Max(i => i.DisplayOrder) + 1 : 1;
      var carImage = new MorentCarImage(this.Id, image.Id, isPrimary: false, displayOrder: nextDisplayOrder);
      _images.Add(carImage);
      return carImage;
    }
  }

  public void RemoveImage(Guid imageId)
  {
    var image = _images.FirstOrDefault(i => i.ImageId == imageId);
    if (image == null)
      throw new DomainException($"Image with ID {imageId} not found for this car");

    _images.Remove(image);

    // If we removed the primary image and there are other images, set the first one as primary
    if (image.IsPrimary && _images.Any())
    {
      _images.First().SetAsPrimary(true);
    }

    // Reorder remaining images
    ReorderImages();
  }

  public void SetPrimaryImage(Guid imageId)
  {
    var newPrimaryImage = _images.FirstOrDefault(i => i.ImageId == imageId);
    if (newPrimaryImage == null)
      throw new DomainException($"Image with ID {imageId} not found for this car");

    foreach (var image in _images)
    {
      image.SetAsPrimary(image.ImageId == imageId);
    }
  }

  private void ReorderImages()
  {
    int order = 1;
    foreach (var image in _images.OrderBy(i => i.DisplayOrder))
    {
      image.UpdateDisplayOrder(order++);
    }
  }

  public void ReorderImage(Guid imageId, int newDisplayOrder)
  {
    var image = _images.FirstOrDefault(i => i.ImageId == imageId);
    if (image == null)
      throw new DomainException($"Image with ID {imageId} not found for this car");

    // Validate new order
    if (newDisplayOrder < 1 || newDisplayOrder > _images.Count)
      throw new DomainException($"Invalid display order. Must be between 1 and {_images.Count}");

    // Get current display order
    int currentOrder = image.DisplayOrder;

    // Update display orders
    if (newDisplayOrder < currentOrder)
    {
      // Moving up in the order
      foreach (var img in _images.Where(i => i.DisplayOrder >= newDisplayOrder && i.DisplayOrder < currentOrder))
      {
        img.UpdateDisplayOrder(img.DisplayOrder + 1);
      }
    }
    else if (newDisplayOrder > currentOrder)
    {
      // Moving down in the order
      foreach (var img in _images.Where(i => i.DisplayOrder <= newDisplayOrder && i.DisplayOrder > currentOrder))
      {
        img.UpdateDisplayOrder(img.DisplayOrder - 1);
      }
    }

    // Set new order for the target image
    image.UpdateDisplayOrder(newDisplayOrder);
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

}
