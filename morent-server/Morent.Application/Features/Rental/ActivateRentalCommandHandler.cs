using System;
using System.ComponentModel;

namespace Morent.Application.Features.Rental;

public class ActivateRentalCommandHandler : ICommandHandler<ActivateRentalCommand, bool>
{
  private readonly IRentalRepository _rentalRepository;
  // private readonly IUnitOfWork _unitOfWork;

  public ActivateRentalCommandHandler(
      // IUnitOfWork unitOfWork,
      IRentalRepository rentalRepository)
  {
    // _unitOfWork = unitOfWork;
    _rentalRepository = rentalRepository;
  }

  public async Task<bool> Handle(ActivateRentalCommand command, CancellationToken cancellationToken)
  {
    var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    if (rental == null)
      throw new ApplicationException($"Rental with ID {command.RentalId} not found");

    rental.ConfirmRental();
    await _rentalRepository.UpdateAsync(rental, cancellationToken);
    await _rentalRepository.SaveChangesAsync(cancellationToken);
    // await _unitOfWork.SaveChangesAsync(cancellationToken);

    return true;
  }
}