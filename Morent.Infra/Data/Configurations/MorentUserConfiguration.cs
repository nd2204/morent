using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentUserConfiguration : IEntityTypeConfiguration<MorentUser>
{
    public void Configure(EntityTypeBuilder<MorentUser> builder)
    {
    }
}
