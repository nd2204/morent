using System;
using Morent.Core.ValueObjects;

namespace Morent.Infrastructure.Data.Configs;

public class MorentCarConfiguration : IEntityTypeConfiguration<MorentCar>
{
  public void Configure(EntityTypeBuilder<MorentCar> builder)
  {
    builder.HasKey(e => e.Id);
    builder.HasOne(x => x.CarModel)
      .WithMany()
      .HasForeignKey(x => x.CarModelId);

    // Configure Money Value Object
    builder.OwnsOne(c => c.PricePerDay, price =>
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

    builder.OwnsOne(c => c.CurrentLocation, c => { 
      c.Property(c => c.Address).IsRequired(); 
      c.Property(c => c.City).IsRequired(); 
      c.Property(c => c.Country).IsRequired(); 
    });

    builder
    .Navigation(c => c.CarModel)
    .AutoInclude();

    // Configure Images collection
    builder.HasMany(e => e.Images)
      .WithOne()
      .HasForeignKey(i => i.CarId);
  }
}
