using Morent.Application.Interfaces;
using Morent.Core.MediaAggregate;
using Morent.Core.ValueObjects;

namespace Morent.Infrastructure.Data.Configs;

public class MorentUserConfiguration : IEntityTypeConfiguration<MorentUser>
{
    public void Configure(EntityTypeBuilder<MorentUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(256);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne<MorentImage>()
            .WithMany()
            .HasForeignKey(u => u.ProfileImageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Email Value Object
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                 .HasColumnName("Email")
                 .IsRequired()
                 .HasMaxLength(256);
        });

        builder.OwnsMany(u => u.RefreshTokens, rt =>
        {
            rt.WithOwner().HasForeignKey("UserId");
            rt.Property<int>("Id").ValueGeneratedOnAdd();
            rt.HasKey("Id");

            rt.Property(t => t.Token).IsRequired();
            rt.Property(t => t.ExpiresAt).IsRequired();
            rt.Property(t => t.CreatedAt).IsRequired();
            rt.Property(t => t.RevokedAt);

            rt.HasIndex(t => t.Token);
        });

        builder.OwnsMany(u => u.OAuthLogins, el =>
        {
            el.WithOwner().HasForeignKey("UserId");
            el.Property<int>("Id").ValueGeneratedOnAdd();
            el.HasKey("Id");

            el.Property(e => e.Provider).IsRequired().HasMaxLength(50);
            el.Property(e => e.ProviderKey).IsRequired().HasMaxLength(200);

            // Add unique index on provider + providerKey
            el.HasIndex(e => new { e.Provider, e.ProviderKey }).IsUnique();
        });
    }
}
