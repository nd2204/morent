using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infrastructure.Data.Configs;

public class MorentCarTypeConfiguration : IEntityTypeConfiguration<MorentCarType>
{
  private static int i = 1;
  public void Configure(EntityTypeBuilder<MorentCarType> builder)
  {
    builder.HasData
    (
      new MorentCarType { Id = i++, Name = "Sport" },
      new MorentCarType { Id = i++, Name = "Suv" },
      new MorentCarType { Id = i++, Name = "MPV" },
      new MorentCarType { Id = i++, Name = "Sedan" },
      new MorentCarType { Id = i++, Name = "Coupe" },
      new MorentCarType { Id = i++, Name = "Hatchback" }
    );
  }
}
