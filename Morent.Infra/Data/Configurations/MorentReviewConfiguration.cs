using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infra.Data.Configurations;

public class MorentReviewConfiguration : IEntityTypeConfiguration<MorentReview>
{
    public void Configure(EntityTypeBuilder<MorentReview> builder)
    {
    }
}
