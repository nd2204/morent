using Morent.Application.Features.Payment.DTOs;

namespace Morent.Application.Features.Payment.Commands;

public record class PaymentCommand(PaymentRequest PaymentRequest) : ICommand<Result<PaymentResponse>>;
