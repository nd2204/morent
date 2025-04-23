using System;

namespace Morent.Infrastructure.Data.Configs;

public class MorentReviewConfiguration : IEntityTypeConfiguration<MorentReview>
{
  public void Configure(EntityTypeBuilder<MorentReview> builder)
  {
    builder.HasKey(e => e.Id);

    builder.HasOne<MorentUser>()
      .WithMany()
      .HasForeignKey(e => e.UserId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne<MorentCar>()
      .WithMany()
      .HasForeignKey(e => e.CarId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.Property(e => e.Rating).IsRequired();
    builder.Property(e => e.Comment).HasMaxLength(1000);
    builder.Property(e => e.CreatedAt).IsRequired();
  }
}
