using System;
using Morent.Core.Events;

namespace Morent.Application.Features.Rental;

public class CompleteRentalCommandHandler : ICommandHandler<CompleteRentalCommand, bool>
{
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;
  private readonly IUnitOfWork _unitOfWork;

  public CompleteRentalCommandHandler(
      IRentalRepository rentalRepository,
      IUnitOfWork unitOfWork,
      ICarRepository carRepository)
  {
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task<bool> Handle(CompleteRentalCommand command, CancellationToken cancellationToken)
  {
    var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    if (rental == null)
      throw new ApplicationException($"Rental with ID {command.RentalId} not found");

    rental.CompleteRental();
    await _rentalRepository.UpdateAsync(rental, cancellationToken);

    // Add domain event
    // var domainEvent = new RentalCompletedEvent(rental.Id, rental.CarId, rental.UserId);

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return true;
  }
}
