using System;

namespace Morent.Application.Features.Rental;

public class CancelRentalCommandHandler : ICommandHandler<CancelRentalCommand, bool>
{
  private readonly IRentalRepository _rentalRepository;
  private readonly IPaymentService _paymentService;
  private readonly ICurrentUserService _currentUserService;
  private readonly IUnitOfWork _unitOfWork;

  public CancelRentalCommandHandler(
      IRentalRepository rentalRepository,
      IPaymentService paymentService,
      ICurrentUserService currentUserService,
      IUnitOfWork unitOfWork)
  {
    _rentalRepository = rentalRepository;
    _paymentService = paymentService;
    _currentUserService = currentUserService;
    _unitOfWork = unitOfWork;
  }

  public async Task<bool> Handle(CancelRentalCommand command, CancellationToken cancellationToken)
  {
    if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
      throw new UnauthorizedAccessException("User must be authenticated to cancel a rental");

    var userId = _currentUserService.UserId.Value;

    var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    if (rental == null)
      throw new ApplicationException($"Rental with ID {command.RentalId} not found");

    // Verify it's the user's rental or user is admin
    if (rental.UserId != userId && _currentUserService.Role != MorentUserRole.Admin)
      throw new UnauthorizedAccessException("You can only cancel your own rentals");

    rental.CancelRental();
    await _rentalRepository.UpdateAsync(rental, cancellationToken);

    // Process refund if needed - for example, full refund if cancelled more than 24h in advance
    var now = DateTime.UtcNow;
    if (rental.RentalPeriod.Start.Subtract(now).TotalHours > 24)
    {
      // Implement refund logic via payment service
      // await _paymentService.RefundPaymentAsync(paymentId);
    }

    // await _unitOfWork.SaveChangesAsync(cancellationToken);
    await _rentalRepository.SaveChangesAsync(cancellationToken);

    return true;
  }
}