using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using Morent.Application.Extensions;

namespace Morent.Application.Features.Rental;

public class RentCarCommandHandler : ICommandHandler<RentCarCommand, Guid>
{
  private readonly ICarRepository _carRepository;
  private readonly IRentalRepository _rentalRepository;
  private readonly IPaymentService _paymentService;
  private readonly IUnitOfWork _unitOfWork;

  public RentCarCommandHandler(
      ICarRepository carRepository,
      IRentalRepository rentalRepository,
      IUnitOfWork unitOfWork,
      IPaymentService paymentService
      )
  {
    _carRepository = carRepository;
    _rentalRepository = rentalRepository;
    _paymentService = paymentService;
    _unitOfWork = unitOfWork;
  }

  public async Task<Guid> Handle(RentCarCommand command, CancellationToken cancellationToken)
  {
    // Validate dates
    if (command.PickupDate >= command.DropoffDate)
      throw new ApplicationException("Pickup date must be before dropoff date");

    if (command.PickupDate.Date < DateTime.UtcNow.Date)
      throw new ApplicationException("Pickup date cannot be in the past");

    // Get car and verify availability
    var car = await _carRepository.GetCarWithRentalsAsync(command.CarId, cancellationToken);
    if (car == null)
      throw new ApplicationException($"Car with ID {command.CarId} not found");

    var rentalPeriod = DateRange.Create(command.PickupDate, command.DropoffDate).Value;

    if (!car.IsAvailableDuring(rentalPeriod))
      throw new ApplicationException("Car is not available for the selected dates");

    // Create pickup and dropoff locations
    var pickupLocation = command.PickupLocation.ToEntity();
    var dropoffLocation = command.DropoffLocation.ToEntity();

    // Calculate total cost
    var durationDays = rentalPeriod.TotalDays();
    var totalCost = car.PricePerDay.Multiply(durationDays);

    // Process payment
    var paymentId = await _paymentService.ProcessPaymentAsync(
        command.UserId,
        Guid.NewGuid(), // Temporary rental ID for payment processing
        totalCost,
        cancellationToken);

    if (string.IsNullOrEmpty(paymentId))
      throw new ApplicationException("Payment processing failed");

    // Create rental
    Guid rentalId = Guid.NewGuid();
    var rental = MorentRental.Create(
        rentalId,
        command.UserId,
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
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return rentalId;
  }
}