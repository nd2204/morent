using System;
using Morent.Application.Features.Payment.DTOs;
using Morent.Application.Interfaces;
using Morent.Core.Exceptions;
using Morent.Core.MorentPaymentAggregate;
using Morent.Core.ValueObjects;

namespace Morent.Infrastructure.Services;

public class PaymentService : IPaymentService
{
  private readonly IDictionary<string, IPaymentProvider> _providers;
  private readonly IImageService _imageService;
  private readonly IRepository<PaymentProvider> _providerRepo;

  public PaymentService(IEnumerable<IPaymentProvider> providers, IImageService imageService, IRepository<PaymentProvider> providerRepo)
  {
    _providers = new Dictionary<string, IPaymentProvider>();

    foreach (var provider in providers)
    {
      _providers.Add(provider.ProviderId, provider);
    }

    _imageService = imageService;
    _providerRepo = providerRepo;
  }

  public async Task<Result<IEnumerable<PaymentMethodDto>>> GetAllProviderDto()
  {
    var methodDtos = new List<PaymentMethodDto>();

    foreach (var provider in _providers.Values) {
      string logoUrl = "";

      var entity = await _providerRepo.GetByIdAsync(provider.ProviderId);
      if (entity == null)
        throw new DomainException($"Provider with Id {provider.ProviderId} not exist in the database");

      if (entity.LogoImageId.HasValue)
      {
        var imageResult = await _imageService.GetImageByIdAsync(entity.LogoImageId.Value);
        logoUrl = imageResult.IsSuccess ? imageResult.Value.Url : logoUrl;
      }

      methodDtos.Add(new PaymentMethodDto
      {
        Id = provider.ProviderId,
        Name = entity.Name,
        LogoUrl = logoUrl,
        FeePercent = entity.FeePercent
      });
    }
    return Result.Success(methodDtos.AsEnumerable());
  }

  public async Task<Result<PaymentResponse>> ProcessPaymentAsync(PaymentRequest request)
  {
    if (!_providers.TryGetValue(request.PaymentProviderId, out var provider))
    {
      throw new NotSupportedException($"Payment provider {request.PaymentProviderId} is not supported");
    }

    return await provider.CreatePaymentAsync(request);
  }

  public async Task<Result<PaymentResponse>> VerifyPaymentAsync(string providerId, IDictionary<string, string> callbackData)
  {
    if (!_providers.TryGetValue(providerId, out var paymentProvider))
    {
      throw new NotSupportedException($"Payment provider {providerId} is not supported");
    }

    return await paymentProvider.VerifyPaymentAsync(callbackData);
  }

  public async Task<Result<PaymentResponse>> RefundPaymentAsync(string providerId, string transactionId, decimal amount)
  {
    if (!_providers.TryGetValue(providerId, out var paymentProvider))
    {
      throw new NotSupportedException($"Payment provider {providerId} is not supported");
    }

    return await paymentProvider.RefundPaymentAsync(transactionId, amount);
  }

}