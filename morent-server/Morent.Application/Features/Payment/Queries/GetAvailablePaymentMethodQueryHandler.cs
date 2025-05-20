using Morent.Application.Features.Payment.DTOs;
using Morent.Core.MorentPaymentAggregate;

namespace Morent.Application.Features.Payment.Queries;

public class GetAvailablePaymentProviderQueryHandler(
  IPaymentService paymentService
): IQueryHandler<GetAvailablePaymentProviderQuery, Result<IEnumerable<PaymentMethodDto>>>
{
  private readonly IPaymentService _paymentService = paymentService;

  public async Task<Result<IEnumerable<PaymentMethodDto>>> Handle(GetAvailablePaymentProviderQuery request, CancellationToken cancellationToken)
  {
    return await _paymentService.GetAllProviderDto();
  }
}
