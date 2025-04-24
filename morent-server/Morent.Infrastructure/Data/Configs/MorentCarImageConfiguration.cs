using System;
using Morent.Core.MediaAggregate;

namespace Morent.Infrastructure.Data.Configs;

public class MorentCarImageConfiguration : IEntityTypeConfiguration<MorentCarImage>
{
  public void Configure(EntityTypeBuilder<MorentCarImage> builder)
  {
    builder.HasKey(e => e.Id);
    builder.Property(e => e.ImageId).IsRequired();
    builder.Property(e => e.IsPrimary).IsRequired();
    builder.Property(e => e.DisplayOrder).IsRequired();
    builder.HasOne<MorentImage>()
      .WithMany()
      .HasForeignKey(c => c.ImageId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
