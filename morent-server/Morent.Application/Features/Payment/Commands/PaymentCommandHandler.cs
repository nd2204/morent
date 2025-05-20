using System;
using Morent.Application.Features.Payment.DTOs;
using Morent.Core.Interfaces;
using Morent.Core.MorentPaymentAggregate.Specifications;

namespace Morent.Application.Features.Payment.Commands;

public class PaymentCommandHandler(
  IRepository<MorentRental> rentalRepo,
  IPaymentService paymentService
) : ICommandHandler<PaymentCommand, Result<PaymentResponse>>
{
  private readonly IRepository<MorentRental> _rentalRepo = rentalRepo;
  private readonly IPaymentService _paymentService = paymentService;

  public async Task<Result<PaymentResponse>> Handle(PaymentCommand request, CancellationToken cancellationToken)
  {
    var rental = await _rentalRepo.GetByIdAsync(request.PaymentRequest.RentalId, cancellationToken);
    if (rental == null)
      return Result.NotFound($"Rental with ID {request.PaymentRequest.RentalId} not found");

    if (rental.Status != MorentRentalStatus.Confirmed)
      return Result.Invalid(new ValidationError("Rental", "Only confirmed rentals can be paid"));

    return await _paymentService.ProcessPaymentAsync(request.PaymentRequest);
  }
}
