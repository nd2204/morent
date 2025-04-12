using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infrastructure.Data.Configs;

public class MorentUserConfiguration : IEntityTypeConfiguration<MorentUser>
{
    public void Configure(EntityTypeBuilder<MorentUser> builder)
    {
    }
}
