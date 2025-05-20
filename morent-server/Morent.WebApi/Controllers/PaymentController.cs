using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.Payment.Commands;
using Morent.Application.Features.Payment.Queries;

namespace Morent.WebApi.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("methods")]
    public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetAvailablePaymentMethods()
    {
        var result = await _mediator.Send(new GetAvailablePaymentProviderQuery());
        return this.ToActionResult(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PaymentResponse>> CreatePayment([FromBody] PaymentRequest request)
    {
        var result = await _mediator.Send(new PaymentCommand(request));
        return this.ToActionResult(result);
    }
}