using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infrastructure.Data.Configs;

public class MorentPaymentConfiguration : IEntityTypeConfiguration<MorentPayment>
{
    public void Configure(EntityTypeBuilder<MorentPayment> builder)
    {
    }
}
