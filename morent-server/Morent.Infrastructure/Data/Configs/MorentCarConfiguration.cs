using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infrastructure.Data.Configs;

public class MorentCarConfiguration : IEntityTypeConfiguration<MorentCar>
{
  public void Configure(EntityTypeBuilder<MorentCar> builder)
  {
    // Car relationships

    // Auto increment
    builder.Property(c => c.Id)
        .ValueGeneratedOnAdd();

    builder.HasOne(c => c.CarModel)
        .WithMany(m => m.Cars)
        .HasForeignKey(c => c.CarModelId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}