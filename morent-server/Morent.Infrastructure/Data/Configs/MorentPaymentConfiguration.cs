using Morent.Core.MorentPaymentAggregate;

namespace Morent.Infrastructure.Data.Configs;

public class MorentPaymentConfiguration : IEntityTypeConfiguration<MorentPayment>
{
  public void Configure(EntityTypeBuilder<MorentPayment> builder)
  {
    builder.HasKey(p => p.Id);
    builder.Property(p => p.Status).IsRequired();
    builder.Property(p => p.RentalId).IsRequired();
    builder.Property(p => p.TransactionId);

    builder.HasOne(p => p.Provider)
      .WithMany()
      .IsRequired();

    // Configure Money Value Object for Amount
    builder.OwnsOne(r => r.PaymentAmount, r =>
    {
      r.Property(p => p.Amount)
        // .HasColumnName("PaymentAmount")
        .HasColumnType("decimal(18,2)")
        .IsRequired();
      r.Property(p => p.Currency)
        // .HasColumnName("PaymentCurrency")
        .HasMaxLength(3)
        .IsRequired();
    });
  }
}
