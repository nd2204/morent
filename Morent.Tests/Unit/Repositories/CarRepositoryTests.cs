using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Morent.Core.MorentCarAggregate;
using Morent.Core.ValueObjects;
using Morent.Infrastructure.Data;
using Morent.Infrastructure.Data.Repositories;

namespace Morent.Tests.Unit.Repositories;

public class CarRepositoryTests : IDisposable
{
    private readonly MorentDbContext _context;
    private readonly CarRepository _repository;
    private readonly List<MorentCar> _testCars = new();

    public CarRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MorentDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MorentDbContext(options);
        _repository = new CarRepository(_context);

        SeedDatabase();
    }

    [Fact]
    public async Task GetCarWithReviewsAsync_WhenCarExists_ReturnsCar()
    {
        // Arrange
        var car = _testCars.First();

        // Act
        var result = await _repository.GetCarWithReviewsAsync(car.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(car.Id);
        result.Reviews.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCarWithReviewsAsync_WhenCarDoesNotExist_ReturnsNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetCarWithReviewsAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAvailableCarsAsync_ReturnsOnlyAvailableCars()
    {
        // Act
        var cars = await _repository.GetAvailableCarsAsync(null, null, null, null, CancellationToken.None);

        // Assert
        cars.Should().NotBeNull();
        cars.Should().OnlyContain(c => c.IsAvailable);
        cars.Count().Should().Be(_testCars.Count(c => c.IsAvailable));
    }

    private void SeedDatabase()
    {
        // Create car models
        var carModel1 = new MorentCarModel(
            Guid.NewGuid(),
            "Toyota",
            "Camry",
            2023,
            FuelType.Gasoline,
            GearBoxType.Automatic,
            CarType.Sedan,
            50,
            5);

        var carModel2 = new MorentCarModel(
            Guid.NewGuid(),
            "Honda",
            "Accord",
            2022,
            FuelType.Hybrid,
            GearBoxType.Automatic,
            CarType.Sedan,
            45,
            5);

        _context.CarModels.Add(carModel1);
        _context.CarModels.Add(carModel2);
        _context.SaveChanges();

        // Create locations
        var location1 = new Location("123 Main St", "New York", "USA");
        var location2 = new Location("456 Park Ave", "Los Angeles", "USA");

        // Create cars
        var car1 = new MorentCar(
            carModel1.Id,
            "ABC123",
            new Money(50.0m, "USD"),
            location1,
            "A comfortable Toyota sedan");
        car1.SetAvailability(true);

        var car2 = new MorentCar(
            carModel2.Id,
            "XYZ789",
            new Money(65.0m, "USD"),
            location2,
            "A spacious Honda sedan");
        car2.SetAvailability(false);

        _context.Cars.Add(car1);
        _context.Cars.Add(car2);
        _context.SaveChanges();

        _testCars.Add(car1);
        _testCars.Add(car2);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 