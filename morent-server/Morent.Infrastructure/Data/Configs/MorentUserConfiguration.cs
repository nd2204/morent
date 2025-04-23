namespace Morent.Infrastructure.Data.Configs;

public class MorentUserConfiguration : IEntityTypeConfiguration<MorentUser>
{
    public void Configure(EntityTypeBuilder<MorentUser> builder)
    {
        builder.OwnsMany(u => u.RefreshTokens, rt => {
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
