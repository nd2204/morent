using System;
using Morent.Core.MediaAggregate;
using Morent.Core.MorentPaymentAggregate;

namespace Morent.Infrastructure.Data.Configs;

public class PaymentMethodConfiguration: IEntityTypeConfiguration<PaymentProvider>
{
  public void Configure(EntityTypeBuilder<PaymentProvider> builder)
  {
    builder.HasKey(p => p.Id);
    builder.Property(p => p.Id).HasMaxLength(64).IsRequired();
    builder.Property(p => p.Name).HasMaxLength(64).IsRequired();
    builder.Property(p => p.FeePercent).IsRequired();

    builder.HasOne<MorentImage>()
        .WithMany()
        .HasForeignKey(u => u.LogoImageId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
