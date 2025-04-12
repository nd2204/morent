using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infrastructure.Data.Configs;

public class MorentImageConfiguration : IEntityTypeConfiguration<MorentImage>
{
    public void Configure(EntityTypeBuilder<MorentImage> builder)
    {
    }
}
