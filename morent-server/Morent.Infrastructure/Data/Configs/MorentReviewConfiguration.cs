using System;

namespace Morent.Infrastructure.Data.Configs;

public class MorentReviewConfiguration : IEntityTypeConfiguration<MorentReview>
{
  public void Configure(EntityTypeBuilder<MorentReview> builder)
  {
    // Configure primary key
    builder.HasKey(e => e.Id);

    // Configure User relationship
    builder.HasOne(r => r.User)
           .WithMany(u => u.Reviews)
           .HasForeignKey(e => e.UserId)
           .OnDelete(DeleteBehavior.Restrict);

    // Configure CarId property explicitly
    builder.Property(e => e.CarId)
           .IsRequired()
           .HasColumnName("CarId");

    // Configure Car relationship
    builder.HasOne(r => r.Car)
           .WithMany(c => c.Reviews)
           .HasForeignKey(e => e.CarId)
           .OnDelete(DeleteBehavior.Restrict);

    // Configure other properties
    builder.Property(e => e.Rating)
           .IsRequired();

    builder.Property(e => e.Comment)
           .HasMaxLength(1000);

    builder.Property(e => e.CreatedAt)
           .IsRequired();

    // Configure navigation (optional AutoInclude)
    builder.Navigation(r => r.User)
           .AutoInclude();
  }
}
