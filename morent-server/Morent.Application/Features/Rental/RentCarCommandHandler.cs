using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;

namespace Morent.Application.Features.Rental;

public class RentCarCommandHandler : ICommandHandler<RentCarCommand, Guid>
{
  private readonly ICarRepository _carRepository;
  private readonly IRentalRepository _rentalRepository;
  private readonly ICurrentUserService _currentUserService;
  private readonly IPaymentService _paymentService;
  private readonly IUnitOfWork _unitOfWork;

  public RentCarCommandHandler(
      ICarRepository carRepository,
      IRentalRepository rentalRepository,
      ICurrentUserService currentUserService,
      IPaymentService paymentService,
      IUnitOfWork unitOfWork)
  {
    _carRepository = carRepository;
    _rentalRepository = rentalRepository;
    _currentUserService = currentUserService;
    _paymentService = paymentService;
    _unitOfWork = unitOfWork;
  }

  public async Task<Guid> Handle(RentCarCommand command, CancellationToken cancellationToken)
  {
    if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
      throw new UnauthorizedAccessException("User must be authenticated to rent a car");

    var userId = _currentUserService.UserId.Value;

    // Validate dates
    if (command.PickupDate >= command.DropoffDate)
      throw new ApplicationException("Pickup date must be before dropoff date");

    if (command.PickupDate.Date < DateTime.UtcNow.Date)
      throw new ApplicationException("Pickup date cannot be in the past");

    // Get car and verify availability
    var car = await _carRepository.GetCarWithRentalsAsync(command.CarId, cancellationToken);
    if (car == null)
      throw new ApplicationException($"Car with ID {command.CarId} not found");

    var rentalPeriod = DateRange.Create(command.PickupDate, command.DropoffDate);

    if (!car.IsAvailableDuring(rentalPeriod))
      throw new ApplicationException("Car is not available for the selected dates");

    // Create pickup and dropoff locations
    var pickupLocation = new Location(
        command.PickupLocation.Address,
        command.PickupLocation.City,
        command.PickupLocation.State,
        command.PickupLocation.ZipCode,
        command.PickupLocation.Country,
        command.PickupLocation.Latitude,
        command.PickupLocation.Longitude);

    var dropoffLocation = new Location(
        command.DropoffLocation.Address,
        command.DropoffLocation.City,
        command.DropoffLocation.State,
        command.DropoffLocation.ZipCode,
        command.DropoffLocation.Country,
        command.DropoffLocation.Latitude,
        command.DropoffLocation.Longitude);

    // Calculate total cost
    var durationDays = rentalPeriod.Value.TotalDays();
    var totalCost = car.PricePerDay.Multiply(durationDays);

    // Process payment
    var paymentId = await _paymentService.ProcessPaymentAsync(
        userId,
        Guid.NewGuid(), // Temporary rental ID for payment processing
        totalCost,
        cancellationToken);

    if (string.IsNullOrEmpty(paymentId))
      throw new ApplicationException("Payment processing failed");

    // Create rental
    Guid rentalId = Guid.NewGuid();
    var rental = MorentRental.Create(
        rentalId,
        userId,
        car.Id,
        rentalPeriod,
        pickupLocation,
        dropoffLocation,
        totalCost);

    // Add rental to car and repository
    car.AddRental(rental);
    await _rentalRepository.AddAsync(rental, cancellationToken);
    await _carRepository.UpdateAsync(car, cancellationToken);

    // Add domain event
    await _carRepository.SaveChangesAsync(cancellationToken);
    // await _unitOfWork.SaveChangesAsync(cancellationToken);

    return rentalId;
  }
}