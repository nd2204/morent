using System;

namespace Morent.UnitTests.Core.MorentCarAggregate;

using Shouldly;
using Morent.Core.Exceptions;
using Morent.Core.MediaAggregate;
using Morent.Core.MorentCarAggregate;
using Morent.Core.ValueObjects;
using System;
using Xunit;

  public class MorentCarTests
  {
    private readonly Guid _modelId = Guid.NewGuid();
    private readonly string _licensePlate = "ABC-123";
    private readonly Money _price = Money.Create(100m).Value;
    private readonly Location _location = Location.Create("123 Main St", "New York", "USA").Value;
    private readonly string _description = "Test car description";

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateCar()
    {
      // Arrange & Act
      var car = new MorentCar(_modelId, _licensePlate, _price, _location, _description);

      // Assert
      car.CarModelId.ShouldBe(_modelId);
      car.LicensePlate.ShouldBe(_licensePlate);
      car.PricePerDay.ShouldBe(_price);
      car.CurrentLocation.ShouldBe(_location);
      car.Description.ShouldBe(_description);
      car.IsAvailable.ShouldBeTrue();
      car.Id.ToString().ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_WithInvalidLicensePlate_ShouldThrowException(string? invalidLicensePlate)
    {
    // Arrange & Act & Assert
#pragma warning disable CS8604 // Possible null reference argument.
    Action act = () => new MorentCar(_modelId, invalidLicensePlate, _price, _location);
#pragma warning restore CS8604 // Possible null reference argument.
    act.ShouldThrow<Exception>();
    }

    [Fact]
    public void Constructor_WithEmptyModelId_ShouldThrowException()
    {
      // Arrange & Act & Assert
      Action act = () => new MorentCar(Guid.Empty, _licensePlate, _price, _location);
      act.ShouldThrow<Exception>();
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var newPrice = Money.Create(150m).Value;

      // Act
      var result = car.UpdatePrice(newPrice);

      // Assert
      result.IsSuccess.ShouldBeTrue();
      car.PricePerDay.ShouldBe(newPrice);
    }

    [Fact]
    public void UpdateDetails_WithAllParameters_ShouldUpdateAllDetails()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var newLicensePlate = "XYZ-789";
      var newPrice = Money.Create(200m).Value;
      var newDescription = "Updated description";

      // Act
      var result = car.UpdateDetails(newLicensePlate, newPrice, newDescription);

      // Assert
      result.IsSuccess.ShouldBeTrue();
      car.LicensePlate.ShouldBe(newLicensePlate);
      car.PricePerDay.ShouldBe(newPrice);
      car.Description.ShouldBe(newDescription);
    }

    [Fact]
    public void UpdateDetails_WithNullParameters_ShouldNotUpdateThoseFields()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location, _description);
      var newLicensePlate = "XYZ-789";

      // Act
      var result = car.UpdateDetails(newLicensePlate, null, null);

      // Assert
      result.IsSuccess.ShouldBeTrue();
      car.LicensePlate.ShouldBe(newLicensePlate);
      car.PricePerDay.ShouldBe(_price);
      car.Description.ShouldBe(_description);
    }

    [Fact]
    public void UpdateLocation_WithValidLocation_ShouldUpdateLocation()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var newLocationResult = Location.Create("456 Elm St", "Chicago", "USA");

      // Act
      car.UpdateLocation(newLocationResult.Value);

      // Assert
      car.CurrentLocation.ShouldBe(newLocationResult.Value);
    }

    [Fact]
    public void SetAvailability_ShouldUpdateAvailabilityStatus()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      car.IsAvailable.ShouldBeTrue(); // Default is true

      // Act
      car.SetAvailability(false);

      // Assert
      car.IsAvailable.ShouldBeFalse();
    }

    [Fact]
    public void AddImage_WithinLimit_ShouldAddImageToCollection()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var mockImage = CreateMockImage();

      // Act
      var carImage = car.AddImage(mockImage, false);

      // Assert
      car.Images.Count.ShouldBe(1);
      car.Images.ShouldContain(carImage);
      carImage.CarId.ShouldBe(car.Id);
      carImage.ImageId.ShouldBe(mockImage.Id);
    }

    [Fact]
    public void AddImage_FirstImage_ShouldSetAsPrimary()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var mockImage = CreateMockImage();

      // Act
      var carImage = car.AddImage(mockImage);

      // Assert
      carImage.IsPrimary.ShouldBeTrue();
      carImage.DisplayOrder.ShouldBe(1);
    }

    [Fact]
    public void AddImage_WithIsPrimaryTrue_ShouldUpdatePrimaryStatus()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var mockImage1 = CreateMockImage();
      var mockImage2 = CreateMockImage();

      // Act
      var carImage1 = car.AddImage(mockImage1); // First image, will be primary
      var carImage2 = car.AddImage(mockImage2, true); // Should become new primary

      // Assert
      car.Images.Count.ShouldBe(2);

      // Need to refetch images as the internal list might have been updated
      var updatedImages = car.Images.ToList();
      var updatedImage1 = updatedImages.FirstOrDefault(i => i.ImageId == mockImage1.Id);
      var updatedImage2 = updatedImages.FirstOrDefault(i => i.ImageId == mockImage2.Id);

      updatedImage1.ShouldNotBeNull();
      updatedImage2.ShouldNotBeNull();

      updatedImage1!.IsPrimary.ShouldBeFalse();
      updatedImage2!.IsPrimary.ShouldBeTrue();
    }

    [Fact]
    public void AddImage_ExceedingLimit_ShouldThrowDomainException()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);

      // Add 10 images (max limit)
      for (int i = 0; i < 10; i++)
      {
        car.AddImage(CreateMockImage());
      }

      // Act & Assert
      Action act = () => car.AddImage(CreateMockImage());
      act.ShouldThrow<DomainException>(
         "*A car cannot have more than 10 images*");
    }

    [Fact]
    public void RemoveImage_ExistingImage_ShouldRemoveFromCollection()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var mockImage = CreateMockImage();
      var carImage = car.AddImage(mockImage);

      // Act
      car.RemoveImage(mockImage.Id);

      // Assert
      car.Images.ShouldBeEmpty();
    }

    [Fact]
    public void RemoveImage_NonExistingImage_ShouldThrowDomainException()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var imageId = Guid.NewGuid();

      // Act & Assert
      Action act = () => car.RemoveImage(imageId);
      act.ShouldThrow<DomainException>(
         $"*Image with ID {imageId} not found for this car*");
    }

    [Fact]
    public void SetPrimaryImage_ShouldUpdatePrimaryStatus()
    {
      // Arrange
      var car = new MorentCar(_modelId, _licensePlate, _price, _location);
      var mockImage1 = CreateMockImage();
      var mockImage2 = CreateMockImage();

      car.AddImage(mockImage1); // Will be primary
      car.AddImage(mockImage2, false); // Will not be primary

      // Act
      car.SetPrimaryImage(mockImage2.Id);

      // Assert
      var images = car.Images.ToList();
      images.Single(i => i.ImageId == mockImage1.Id).IsPrimary.ShouldBeFalse();
      images.Single(i => i.ImageId == mockImage2.Id).IsPrimary.ShouldBeTrue();
    }

    private MorentImage CreateMockImage()
    {
      // Create a basic image entity with required properties
      var imageId = Guid.NewGuid();
      var image = new MorentImage("test-image.jpg", "image/jpeg", "testurl.com/image.jpg", 100);

      // Use reflection to set the Id since it's likely protected
      typeof(MorentImage)
          .GetProperty("Id")
          ?.SetValue(image, imageId);

      return image;
    }
  }