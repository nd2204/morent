using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Morent.Api.Controllers;
using Morent.Application.Features.Car;
using Morent.Application.Features.Car.Commands;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Features.Car.Queries;
using Morent.Application.Features.DTOs;
using Morent.Infrastructure.Data;
using Morent.Application.Features.Review.DTOs;

namespace Morent.Tests.Controllers;

public class CarsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<CarsController>> _loggerMock;
    private readonly Mock<MorentDbContext> _contextMock;
    private readonly CarsController _controller;

    public CarsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<CarsController>>();
        _contextMock = new Mock<MorentDbContext>();
        _controller = new CarsController(_contextMock.Object, _mediatorMock.Object, _loggerMock.Object);
        
        // Setup HttpContext for response headers
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetCars_ReturnsPagedResults_WithCorrectHeaders()
    {
        // Arrange
        var query = new GetCarsQuery
        {
            pagedQuery = new PagedQuery { Page = 1, PageSize = 10 },
            carFilter = new CarFilter { Brand = "Toyota" }
        };
        
        var cars = new List<CarDto>
        {
            new CarDto
            {
                Id = Guid.NewGuid(),
                LicensePlate = "ABC123",
                PricePerDay = 50.0m,
                Currency = "USD",
                IsAvailable = true,
                AverageRating = 4.5,
                ReviewsCount = 10,
                CarModel = new CarModelDto
                {
                    Brand = "Toyota",
                    Model = "Camry",
                    Year = 2023,
                    FuelType = "Gasoline",
                    GearBox = "Automatic",
                    SeatCapacity = 5
                }
            },
            new CarDto
            {
                Id = Guid.NewGuid(),
                LicensePlate = "DEF456",
                PricePerDay = 60.0m,
                Currency = "USD",
                IsAvailable = true,
                AverageRating = 4.2,
                ReviewsCount = 8,
                CarModel = new CarModelDto
                {
                    Brand = "Toyota",
                    Model = "Corolla",
                    Year = 2022,
                    FuelType = "Gasoline",
                    GearBox = "Automatic",
                    SeatCapacity = 5
                }
            }
        };
        
        var pagedInfo = new PagedInfo(1, 10, 1, 2);
        var pagedResult = new PagedResult<IEnumerable<CarDto>>(pagedInfo, cars);
        
        _mediatorMock.Setup(m => m.Send(
            It.Is<GetCarsByQuery>(q => q.query == query),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetCars(query);

        // Assert
        // Check response
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCars = okResult.Value.Should().BeAssignableTo<IEnumerable<CarDto>>().Subject;
        returnedCars.Should().HaveCount(2);
        
        // Check headers
        var headers = _controller.Response.Headers;
        headers.Should().ContainKey("X-Total-Count");
        headers["X-Total-Count"].ToString().Should().Be("2");
        headers.Should().ContainKey("X-Page-Number");
        headers["X-Page-Number"].ToString().Should().Be("1");
        headers.Should().ContainKey("X-Page-Size");
        headers["X-Page-Size"].ToString().Should().Be("10");
        headers.Should().ContainKey("X-Total-Pages");
        headers["X-Total-Pages"].ToString().Should().Be("1");
    }

    [Fact]
    public async Task GetCarById_WhenCarExists_ReturnsOkWithCar()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var carDetailDto = new CarDetailDto
        {
            Id = carId,
            LicensePlate = "ABC123",
            PricePerDay = 50.0m,
            Currency = "USD",
            IsAvailable = true,
            AverageRating = 4.5,
            ReviewsCount = 10,
            Description = "A nice car for rent",
            Reviews = new List<ReviewDto>(),
            Location = new CarLocationDto
            {
                City = "New York",
                Address = "123 Main St",
                Country = "USA"
            }
        };

        var expectedResult = Result.Success(carDetailDto);

        _mediatorMock.Setup(m => m.Send(
            It.Is<GetCarByIdQuery>(q => q.Id == carId),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetCarById(carId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCar = okResult.Value.Should().BeOfType<CarDetailDto>().Subject;
        returnedCar.Should().BeEquivalentTo(carDetailDto);
    }

    [Fact]
    public async Task GetCarById_WhenCarDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var expectedResult = Result.NotFound($"Car with ID {carId} not found");

        _mediatorMock.Setup(m => m.Send(
            It.Is<GetCarByIdQuery>(q => q.Id == carId),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetCarById(carId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    [Fact]
    public async Task CreateCar_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var modelId = Guid.NewGuid();
        var command = new CreateCarCommand
        {
            ModelId = modelId,
            LicensePlate = "XYZ789",
            PricePerDay = 75.0m,
            Currency = "USD",
            Location = new CarLocationDto 
            {
                City = "Miami",
                Address = "789 Ocean Dr",
                Country = "USA"
            },
            Images = new List<UploadCarImageRequest>()
        };
        
        var resultId = Guid.NewGuid();
        var expectedResult = Result.Success(resultId);
        
        _mediatorMock.Setup(m => m.Send(
            It.IsAny<CreateCarCommand>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateCar(command);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(resultId);
    }
    
    [Fact]
    public async Task UpdateCar_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var carId = Guid.NewGuid();
        var command = new UpdateCarCommand
        {
            Id = carId,
            PricePerDay = 75.0m,
            Currency = "USD",
            IsAvailable = true,
            Location = new CarLocationDto
            {
                City = "Miami",
                Address = "789 Ocean Dr",
                Country = "USA"
            },
            ImagesToAdd = new List<UploadCarImageRequest>(),
            ImagesToDelete = new List<Guid>()
        };
        
        _mediatorMock.Setup(m => m.Send(
            It.IsAny<UpdateCarCommand>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateCar(carId, command);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(true);
    }
    
    [Fact]
    public async Task DeleteCar_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var carId = Guid.NewGuid();
        
        _mediatorMock.Setup(m => m.Send(
            It.Is<DeleteCarCommand>(c => c.carId == carId),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteCar(carId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(true);
    }
} 