using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentAssertions;
using Moq;
using Morent.Application.Features.Car;
using Morent.Application.Repositories;
using Morent.Core.MorentCarAggregate;
using Morent.Core.ValueObjects;

namespace Morent.Tests.Unit.Features.Car;

public class GetCarByIdQueryHandlerTests
{
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly GetCarByIdQueryHandler _handler;

    public GetCarByIdQueryHandlerTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _handler = new GetCarByIdQueryHandler(_carRepositoryMock.Object, _reviewRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCarExists_ReturnsSuccessResult()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var car = CreateSampleCar();
        
        _carRepositoryMock.Setup(repo => repo.GetCarWithReviewsAsync(
            carId, 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(car);

        var query = new GetCarByIdQuery(carId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(car.Id);
    }

    [Fact]
    public async Task Handle_WhenCarDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var carId = Guid.NewGuid();
        
        _carRepositoryMock.Setup(repo => repo.GetCarWithReviewsAsync(
            carId, 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((MorentCar)null);

        var query = new GetCarByIdQuery(carId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(Ardalis.Result.ResultStatus.NotFound);
        result.Errors.Should().ContainSingle(error => error.Contains(carId.ToString()));
    }

    private MorentCar CreateSampleCar()
    {
        // Create a car model first
        var modelId = Guid.NewGuid();
        var carModel = new MorentCarModel(
            modelId,
            "Toyota", 
            "Camry", 
            2023, 
            FuelType.Gasoline, 
            GearBoxType.Automatic, 
            CarType.Sedan,
            50, // Fuel tank capacity
            5   // Seat capacity
        );
            
        var location = new Location("123 Main St", "New York", "USA");
        var price = new Money(50.0m, "USD");
        
        // The MorentCar constructor generates its own Id internally,
        // so we don't need to explicitly set it
        var car = new MorentCar(
            modelId,
            "ABC123", 
            price,
            location,
            "A comfortable sedan for rent");
            
        return car;
    }
} 