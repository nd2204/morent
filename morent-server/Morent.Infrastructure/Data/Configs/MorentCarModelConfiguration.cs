using System;

namespace Morent.Infrastructure.Data.Configs;

public class MorentCarModelConfiguration : IEntityTypeConfiguration<MorentCarModel>
{
  public void Configure(EntityTypeBuilder<MorentCarModel> builder)
  {
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Brand)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(x => x.ModelName)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(x => x.Year)
        .IsRequired();

    builder.Property(x => x.FuelType)
        .IsRequired()
        .HasConversion<string>() // Save enum as string (optional, good for readability)
        .HasMaxLength(50);

    builder.Property(x => x.Gearbox)
        .IsRequired()
        .HasMaxLength(50);

    builder.Property(x => x.SeatCapacity)
        .IsRequired();

    builder.Property(x => x.CarType)
        .HasConversion<string>()
        .IsRequired();

    // Optional: Unique Constraint to prevent duplicate CarModels
    builder.HasIndex(x => new { x.Brand, x.ModelName, x.Year })
        .IsUnique();
  }
}
