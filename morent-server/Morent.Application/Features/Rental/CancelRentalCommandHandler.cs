using System;

namespace Morent.Application.Features.Rental;

public class CancelRentalCommandHandler : ICommandHandler<CancelRentalCommand, bool>
{
  private readonly IRentalRepository _rentalRepository;
  private readonly IPaymentService _paymentService;
  private readonly IUnitOfWork _unitOfWork;

  public CancelRentalCommandHandler(
      IRentalRepository rentalRepository,
      IUnitOfWork unitOfWork,
      IPaymentService paymentService)
  {
    _rentalRepository = rentalRepository;
    _paymentService = paymentService;
    _unitOfWork = unitOfWork;
  }

  public async Task<bool> Handle(CancelRentalCommand command, CancellationToken cancellationToken)
  {
    var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
    if (rental == null)
      throw new ApplicationException($"Rental with ID {command.RentalId} not found");

    if (rental.UserId != command.UserId)
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

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return true;
  }
}