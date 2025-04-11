using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentLocationConfiguration : IEntityTypeConfiguration<MorentLocation>
{
    public void Configure(EntityTypeBuilder<MorentLocation> builder)
    {
    }
}
