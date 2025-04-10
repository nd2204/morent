using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentFavoriteConfiguration : IEntityTypeConfiguration<MorentFavorite>
{
  public void Configure(EntityTypeBuilder<MorentFavorite> builder)
  {
    builder.HasKey(cf => new { cf.UserId, cf.CarId });

    builder.HasOne(cf => cf.User)
        .WithMany(u => u.Favorites)
        .HasForeignKey(cf => cf.UserId);

    builder.HasOne(cf => cf.Car)
        .WithMany(c => c.Favorites)
        .HasForeignKey(cf => cf.CarId);
  }
}
