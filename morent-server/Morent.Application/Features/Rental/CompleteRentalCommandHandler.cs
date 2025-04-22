using System;
using Morent.Core.Events;

namespace Morent.Application.Features.Rental;

public class CompleteRentalCommandHandler : ICommandHandler<CompleteRentalCommand, bool>
{
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;
  private readonly ICurrentUserService _currentUserService;
  private readonly IUnitOfWork _unitOfWork;

  public CompleteRentalCommandHandler(
      IRentalRepository rentalRepository,
      ICarRepository carRepository,
      ICurrentUserService currentUserService,
      IUnitOfWork unitOfWork)
  {
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
    _currentUserService = currentUserService;
    _unitOfWork = unitOfWork;
  }

  public async Task<bool> Handle(CompleteRentalCommand command, CancellationToken cancellationToken)
  {
    // Verify user is an admin or support
    if (_currentUserService.Role != MorentUserRole.Admin)
      throw new UnauthorizedAccessException("Only admin or support can complete rentals");

    var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    if (rental == null)
      throw new ApplicationException($"Rental with ID {command.RentalId} not found");

    rental.CompleteRental();
    await _rentalRepository.UpdateAsync(rental, cancellationToken);

    // Add domain event
    var domainEvent = new RentalCompletedEvent(rental.Id, rental.CarId, rental.UserId);

    // await _unitOfWork.SaveChangesAsync(cancellationToken);
    await _rentalRepository.SaveChangesAsync(cancellationToken);

    return true;
  }
}
