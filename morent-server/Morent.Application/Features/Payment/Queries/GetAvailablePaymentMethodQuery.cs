using System;
using Morent.Application.Features.Payment.DTOs;

namespace Morent.Application.Features.Payment.Queries;

public record class GetAvailablePaymentProviderQuery() : IQuery<Result<IEnumerable<PaymentMethodDto>>>;
