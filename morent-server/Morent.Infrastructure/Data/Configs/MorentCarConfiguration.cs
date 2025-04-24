using System;

namespace Morent.Infrastructure.Data.Configs;

public class MorentCarConfiguration : IEntityTypeConfiguration<MorentCar>
{
  public void Configure(EntityTypeBuilder<MorentCar> builder)
  {
    builder.HasKey(e => e.Id);
    builder.Property(e => e.Brand).IsRequired().HasMaxLength(50);
    builder.Property(e => e.Model).IsRequired().HasMaxLength(100);
    builder.Property(e => e.FuelType).IsRequired().HasMaxLength(30);
    builder.Property(e => e.Capacity).IsRequired();

    // Configure Money Value Object
    builder.ComplexProperty(c => c.PricePerDay, price =>
    {
      price.Property(p => p.Amount)
        .HasColumnName("PriceAmount")
        .HasColumnType("decimal(18,2)")
        .IsRequired();

      price.Property(p => p.Currency)
        .HasColumnName("PriceCurrency")
        .HasMaxLength(3)
        .IsRequired();
    });

    builder.ComplexProperty(c => c.CurrentLocation, c => { c.IsRequired(); });

    // Configure Images collection
    builder.Property(e => e.Images)
      .HasConversion(
        v => string.Join(',', v),
        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
  }
}
