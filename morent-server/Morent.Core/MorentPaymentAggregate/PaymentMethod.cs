namespace Morent.Core.MorentPaymentAggregate;

public class PaymentProvider : IAggregateRoot
{
  public string Id;
  public Guid? LogoImageId { get; set; }
  public string Name { get; private set; }
  public decimal FeePercent { get; private set; }

  public PaymentProvider(string id, string name, Guid? logoImageId, decimal feePercent)
  {
    Guard.Against.NullOrEmpty(id);
    Id = id;
    Name = name;
    LogoImageId = logoImageId;
    FeePercent = feePercent;
  }
}
