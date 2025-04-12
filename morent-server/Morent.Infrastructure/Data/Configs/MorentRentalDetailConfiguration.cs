using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infrastructure.Data.Configs;

public class MorentRentalDetailConfiguration : IEntityTypeConfiguration<MorentRentalDetail>
{
  public void Configure(EntityTypeBuilder<MorentRentalDetail> builder)
  {
  }
}
