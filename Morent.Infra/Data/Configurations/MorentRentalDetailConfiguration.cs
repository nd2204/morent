using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentRentalDetailConfiguration : IEntityTypeConfiguration<MorentRentalDetail>
{
  public void Configure(EntityTypeBuilder<MorentRentalDetail> builder)
  {
    // RentalDetail relationships
    builder.HasOne(rd => rd.PickupLocation)
        .WithMany()
        .HasForeignKey(rd => rd.PickupLocationId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(rd => rd.DropoffLocation)
        .WithMany()
        .HasForeignKey(rd => rd.DropoffLocationId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
