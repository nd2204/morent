using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morent.Core.Entities;

namespace Morent.Infrastructure.Data.Configs;

public class MorentReviewConfiguration : IEntityTypeConfiguration<MorentReview>
{
    public void Configure(EntityTypeBuilder<MorentReview> builder)
    {
    }
}
