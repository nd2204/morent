using System;
using System.ComponentModel;

namespace Morent.Application.Features.Rental;

public class ActivateRentalCommandHandler : ICommandHandler<ActivateRentalCommand, bool>
{
  private readonly IRentalRepository _rentalRepository;
  private readonly ICurrentUserService _currentUserService;
  // private readonly IUnitOfWork _unitOfWork;

  public ActivateRentalCommandHandler(
      IRentalRepository rentalRepository,
      // IUnitOfWork unitOfWork,
      ICurrentUserService currentUserService)
  {
    _rentalRepository = rentalRepository;
    _currentUserService = currentUserService;
    // _unitOfWork = unitOfWork;
  }

  public async Task<bool> Handle(ActivateRentalCommand command, CancellationToken cancellationToken)
  {
    // Verify user is an admin
    if (_currentUserService.Role != MorentUserRole.Admin)
      throw new UnauthorizedAccessException("Only admin or support can activate rentals");

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