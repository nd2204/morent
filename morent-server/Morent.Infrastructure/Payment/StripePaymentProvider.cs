using System;
using Microsoft.Extensions.Options;
using Morent.Application.Features.Payment.DTOs;
using Morent.Application.Repositories;
using Morent.Core.Exceptions;
using Morent.Core.MorentPaymentAggregate;
using Morent.Infrastructure.Settings;
using Stripe;
using Stripe.Checkout;

namespace Morent.Infrastructure.Payment;

public class StripePaymentProvider : IPaymentProvider
{
  public static string Id = "stripe";
  public string ProviderId => Id;
  private readonly StripeSettings _options;
  private readonly IRentalRepository _rentalRepo;

  public StripePaymentProvider(IOptions<StripeSettings> options, IRentalRepository rentalRepo)
  {
    _options = options.Value;
    StripeConfiguration.ApiKey = _options.SecretKey;
    _rentalRepo = rentalRepo;
  }

  public async Task<Result<PaymentResponse>> CreatePaymentAsync(PaymentRequest request)
  {
    try
    {
      var rental = await _rentalRepo.GetByIdAsync(request.RentalId);
      if (rental == null)
        return Result.NotFound($"Rental with ID {request.RentalId} not found");

      // Convert the amount to the smallest currency unit (cents)
      long unitAmount = Convert.ToInt64(rental.TotalCost.Multiply(100).Value.Amount);

      // Create a new Stripe checkout session
      var options = new SessionCreateOptions
      {
        PaymentMethodTypes = new List<string> { "card" },
        LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {
              UnitAmount = unitAmount,
              Currency = rental.TotalCost.Currency.ToLower(),
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = $"Rental Id {rental.Id}",
                Description = request.Description
              }
            },
            Quantity = 1
          }
        },
        Mode = "payment",
        SuccessUrl = $"{request.ReturnUrl}?session_id={{CHECKOUT_SESSION_ID}}&status=success",
        CancelUrl = $"{request.ReturnUrl}?status=cancel",
        ClientReferenceId = rental.Id.ToString(),
        Metadata = request.Metadata
      };

      var service = new SessionService();
      var session = await service.CreateAsync(options);

      return Result.Created(new PaymentResponse
      {
        TransactionId = session.Id,
        Status = PaymentStatus.Pending,
        PaymentUrl = session.Url,
        Message = "Payment session created successfully",
        ProviderData = new Dictionary<string, string>
        {
          { "SessionId", session.Id },
          { "PaymentIntentId", session.PaymentIntentId }
        }
      });
    }
    catch (StripeException ex)
    {
      return Result.Error(ex.Message);
    }
  }

  public async Task<Result<PaymentResponse>> VerifyPaymentAsync(IDictionary<string, string> callbackData)
  {
    try
    {
      if (!callbackData.TryGetValue("session_id", out var sessionId))
      {
        return Result.Error("Session ID not found in callback data");
      }

      var service = new SessionService();
      var session = await service.GetAsync(sessionId);

      if (session.PaymentStatus == "paid")
      {
        return Result.Success(new PaymentResponse
        {
          TransactionId = session.Id,
          Status = PaymentStatus.Completed,
          Message = "Payment completed successfully",
          ProviderData = new Dictionary<string, string>
          {
            { "SessionId", session.Id },
            { "PaymentIntentId", session.PaymentIntentId },
            { "CustomerEmail", session.CustomerEmail },
            { "OrderId", session.ClientReferenceId }
          }
        });
      }
      else
      {
        return Result.Created(new PaymentResponse
        {
          TransactionId = session.Id,
          Status = PaymentStatus.Pending,
          Message = $"Payment status: {session.PaymentStatus}",
          ProviderData = new Dictionary<string, string>
          {
            { "SessionId", session.Id },
            { "PaymentIntentId", session.PaymentIntentId },
            { "OrderId", session.ClientReferenceId }
          }
        });
      }
    }
    catch (StripeException ex)
    {
      return Result.Error(ex.Message);
    }
  }

  public async Task<Result<PaymentResponse>> RefundPaymentAsync(string transactionId, decimal amount)
  {
    try
    {
      // In Stripe, we need the PaymentIntent ID to process refunds
      // First, get the session to retrieve the PaymentIntent ID
      var sessionService = new SessionService();
      var session = await sessionService.GetAsync(transactionId);

      if (string.IsNullOrEmpty(session.PaymentIntentId))
      {
        return Result.Error("No payment intent associated with this session");
      }

      // Create the refund
      var refundOptions = new RefundCreateOptions
      {
        PaymentIntent = session.PaymentIntentId,
        Amount = Convert.ToInt64(amount * 100), // Convert to cents
        Reason = "requested_by_customer"
      };

      var refundService = new RefundService();
      var refund = await refundService.CreateAsync(refundOptions);

      return Result.Success(new PaymentResponse
      {
        TransactionId = refund.Id,
        Status = PaymentStatus.Refunded,
        Message = "Refund processed successfully",
        ProviderData = new Dictionary<string, string>
        {
          { "RefundId", refund.Id },
          { "PaymentIntentId", refund.PaymentIntentId },
          { "Status", refund.Status }
        }
      });
    }
    catch (StripeException ex)
    {
      return Result.Error(ex.Message);
    }
  }
}