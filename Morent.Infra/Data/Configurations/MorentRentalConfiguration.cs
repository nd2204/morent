using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentRentalConfiguration : IEntityTypeConfiguration<MorentRental>
{
  public void Configure(EntityTypeBuilder<MorentRental> builder)
  {
    builder.HasOne(r => r.User)
        .WithMany(u => u.Rentals)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(r => r.Car)
        .WithMany(c => c.Rentals)
        .HasForeignKey(r => r.CarId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(r => r.RentalDetail)
        .WithOne(rd => rd.Rental)
        .HasForeignKey<MorentRentalDetail>(rd => rd.RentalId);
  }
}