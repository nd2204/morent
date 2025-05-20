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

    builder.OwnsOne(c => c.CurrentLocation, location => {
      location.Property(l => l.Address).HasColumnName("LocationAddress");
      location.Property(l => l.City).HasColumnName("LocationCity");
      location.Property(l => l.Country).HasColumnName("LocationCountry");
      location.Property(l => l.Longitude).HasColumnName("LocationLongitude");
      location.Property(l => l.Latitude).HasColumnName("LocationLatitude");

      // Optional: Configure string lengths or other constraints
      location.Property(l => l.Address).HasMaxLength(200);
      location.Property(l => l.City).HasMaxLength(100);
      location.Property(l => l.Country).HasMaxLength(100);

      // Optional: Allow nulls for string properties if needed
      location.Property(l => l.Address).IsRequired(false);
      location.Property(l => l.City).IsRequired(false);
      location.Property(l => l.Country).IsRequired(false);
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
