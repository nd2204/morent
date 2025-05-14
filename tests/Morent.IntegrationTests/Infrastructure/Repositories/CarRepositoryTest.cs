using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Repositories;
using Morent.Core.MorentCarAggregate;
using Morent.Core.MorentRentalAggregate;
using Morent.IntegrationTests.Data;
using Shouldly;

namespace Morent.IntegrationTests.Infrastructure.Repositories;
public class CarRepositoryIntegrationTests : BaseEfRepoTestFixture, IDisposable
{
  private readonly ICarRepository _carRepository;
  public CarRepositoryIntegrationTests()
  {
    _carRepository = GetCarRepository();
    // Seed test data
    SeedData.SeedCars(_dbContext).GetAwaiter().GetResult();
  }

  [Fact]
  public async Task GetCarModelsByQuery_WithBrandOnly_ShouldReturnMatchingModels()
  {
    // Arrange
    var brand = SeedData.ToyotaHilux.Brand;
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetCarModelsByQuery(brand, null, null, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    foreach(var car in result) {
      car.Brand.ShouldContain(brand);
    }
  }

  [Fact]
  public async Task GetCarModelsByQuery_WithBrandAndModel_ShouldReturnMatchingModels()
  {
    // Arrange
    var brand = "Honda";
    var modelName = "CR-V";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetCarModelsByQuery(brand, modelName, null, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    result.Count().ShouldBe(1);
    result.First().Brand.ShouldBe(brand);
    result.First().ModelName.ShouldBe(modelName);
  }

  [Fact]
  public async Task GetCarModelsByQuery_WithYear_ShouldReturnMatchingModels()
  {
    // Arrange
    var year = 2023;
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetCarModelsByQuery(null, null, year, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    result.First().Year.ShouldBe(year);
  }

  [Fact]
  public async Task GetAvailableCarsAsync_WithNoParameters_ShouldReturnAvailableCars()
  {
    // Arrange
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetAvailableCarsAsync(null, null, null, null, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    result.Count().ShouldBe(SeedData.NUM_CARS_TO_SEED);
    result.All(c => c.IsAvailable).ShouldBeTrue();
  }

  [Fact]
  public async Task GetAvailableCarsAsync_WithDateRange_ShouldExcludeRentedCars()
  {
    // Arrange
    var now = DateTime.UtcNow;
    var start = now;
    var end = now.AddDays(2);
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetAvailableCarsAsync(start, end, null, null, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
  }

  [Fact]
  public async Task GetAvailableCarsAsync_WithCapacity_ShouldFilterByCapacity()
  {
    // Arrange
    var minCapacity = 5;
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetAvailableCarsAsync(null, null, null, minCapacity, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    result.All(c => c.CarModel.SeatCapacity >= minCapacity).ShouldBeTrue();
  }

  // Not yet implemented
  // [Fact]
  // public async Task GetAvailableCarsAsync_WithLocation_ShouldReturnNearestCars()
  // {
  //   // Arrange
  //   var nearLocation = new CarLocationDto
  //   {
  //     Latitude = 34.0522,  // Los Angeles
  //     Longitude = -118.2437
  //   };
  //   var cancellationToken = CancellationToken.None;

  //   // Act
  //   var result = await _carRepository.GetAvailableCarsAsync(null, null, nearLocation, null, cancellationToken);

  //   // Assert
  //   result.ShouldNotBeNull();
  //   result.ShouldNotBeEmpty();

  //   // The Los Angeles car should be first (closest)
  //   result.First().Id.ShouldBe(_testCarIds[0]);
  // }

  [Fact]
  public async Task GetCarsByBrandAsync_WithExistingBrand_ShouldReturnMatchingCars()
  {
    // Arrange
    var brand = SeedData.ToyotaHilux.Brand;
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetCarsByBrandAsync(brand, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    foreach (var car in result)
    {
      car.CarModel.Brand.ShouldBe(brand);
    }
  }

  [Fact]
  public async Task GetCarsByBrandAsync_WithNonExistingBrand_ShouldReturnEmptyList()
  {
    // Arrange
    var brand = "Toyotaa";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetCarsByBrandAsync(brand, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldBeEmpty();
  }

  [Fact]
  public async Task GetCarsByModelAsync_WithExistingModelName_ShouldReturnMatchingCars()
  {
    // Arrange
    var carModel = SeedData.KoenigseggCCGT;
    var modelName = carModel.ModelName;
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetCarsByModelAsync(modelName, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.ShouldNotBeEmpty();
    foreach (var car in result)
    {
      car.CarModel.ModelName.ShouldContain(modelName);
    }
  }

  // [Fact]
  // public async Task GetCarWithRentalsAsync_WithExistingCar_ShouldIncludeRentals()
  // {
  //   // Arrange
  //   var car = _testCars.First();
  //   var cancellationToken = CancellationToken.None;

  //   // Act
  //   var result = await _carRepository.GetCarWithRentalsAsync(car.Id, cancellationToken);

  //   // Assert
  //   result.ShouldNotBeNull();
  //   result.Id.ShouldBe(car.Id);
  //   result.Rentals.ShouldNotBeNull();
  //   result.Rentals.ShouldNotBeEmpty();
  //   result.Rentals.Count.ShouldBe(2); // Car1 has 2 rentals (1 active, 1 completed)
  // }

  [Fact]
  public async Task GetCarWithRentalsAsync_WithNonExistingCar_ShouldReturnNull()
  {
    // Arrange
    var carId = Guid.NewGuid(); // Non-existing car
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _carRepository.GetCarWithRentalsAsync(carId, cancellationToken);

    // Assert
    result.ShouldBeNull();
  }

  // [Fact]
  // public async Task GetCarWithReviewsAsync_WithExistingCar_ShouldIncludeReviews()
  // {
  //   // Arrange
  //   var carId = _testCarIds[0]; // Car1 has reviews
  //   var cancellationToken = CancellationToken.None;

  //   // Act
  //   var result = await _carRepository.GetCarWithReviewsAsync(carId, cancellationToken);

  //   // Assert
  //   result.ShouldNotBeNull();
  //   result.Id.ShouldBe(carId);
  //   result.Reviews.ShouldNotBeNull();
  //   result.Reviews.ShouldNotBeEmpty();
  //   result.Reviews.Count.ShouldBe(1); // Car1 has 1 review
  // }

  // [Fact]
  // public async Task GetCarWithImagesAsync_WithExistingCar_ShouldIncludeImages()
  // {
  //   // Arrange
  //   var carId = _testCarIds[0]; // Car1 has images
  //   var cancellationToken = CancellationToken.None;

  //   // Act
  //   var result = await _carRepository.GetCarWithImagesAsync(carId, cancellationToken);

  //   // Assert
  //   result.ShouldNotBeNull();
  //   result.Id.ShouldBe(carId);
  //   result.Images.ShouldNotBeNull();
  //   result.Images.ShouldNotBeEmpty();
  //   result.Images.Count.ShouldBe(2); // Car1 has 2 images
  // }

  // [Fact]
  // public async Task GetActiveRentalsForCarAsync_WithExistingCar_ShouldReturnActiveRentals()
  // {
  //   // Arrange
  //   var carId = _testCarIds[0]; // Car1 has an active rental
  //   var cancellationToken = CancellationToken.None;

  //   // Act
  //   var result = await _carRepository.GetActiveRentalsForCarAsync(carId, cancellationToken);

  //   // Assert
  //   result.ShouldNotBeNull();
  //   result.ShouldNotBeEmpty();
  //   result.Count().ShouldBe(1); // Car1 has 1 active rental
  //   result.All(r => r.Status == MorentRentalStatus.Active).ShouldBeTrue();
  // }

  // [Fact]
  // public async Task GetActiveRentalsForCarAsync_WithNoActiveRentals_ShouldReturnEmptyList()
  // {
  //   // Arrange
  //   var carId = _testCarIds[2]; // Car3 has no rentals
  //   var cancellationToken = CancellationToken.None;

  //   // Act
  //   var result = await _carRepository.GetActiveRentalsForCarAsync(carId, cancellationToken);

  //   // Assert
  //   result.ShouldNotBeNull();
  //   result.ShouldBeEmpty();
  // }

  public void Dispose()
  {
    // Clean up the in-memory database after each test
    _dbContext.Database.EnsureDeleted();
    _dbContext.Dispose();
  }
}