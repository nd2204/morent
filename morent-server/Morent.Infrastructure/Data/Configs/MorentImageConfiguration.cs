using System;
using Morent.Core.MediaAggregate;

namespace Morent.Infrastructure.Data.Configs;

public class MorentImageConfiguration : IEntityTypeConfiguration<MorentImage>
{
  public void Configure(EntityTypeBuilder<MorentImage> builder)
  {
    builder.HasKey(e => e.Id);
    builder.Property(e => e.FileName).IsRequired().HasMaxLength(255);
    builder.Property(e => e.ContentType).IsRequired().HasMaxLength(50);
    builder.Property(e => e.Path).IsRequired().HasMaxLength(500);
    builder.Property(e => e.UploadedAt).IsRequired();
    builder.Property(e => e.Size).IsRequired();
  }
}
