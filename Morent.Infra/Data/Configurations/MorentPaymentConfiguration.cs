using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentPaymentConfiguration : IEntityTypeConfiguration<MorentPayment>
{
    public void Configure(EntityTypeBuilder<MorentPayment> builder)
    {
    }
}
