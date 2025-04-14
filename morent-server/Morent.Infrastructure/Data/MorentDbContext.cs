using Morent.Infrastructure.Data.Configs;

namespace Morent.Infrastructure.Data;

public class MorentDbContext : DbContext
{
  public MorentDbContext(DbContextOptions<MorentDbContext> options)
      : base(options)
  {
  }

  public DbSet<MorentUser> Users => Set<MorentUser>();
  // public DbSet<MorentOwnedCar> OwnedCars => Set<MorentOwnedCar>();
  // public DbSet<MorentCar> Cars => Set<MorentCar>();
  // public DbSet<MorentRental> Rentals => Set<MorentRental>();
  // public DbSet<MorentRentalDetail> RentalDetails => Set<MorentRentalDetail>();
  // public DbSet<MorentPayment> Payments => Set<MorentPayment>();
  // public DbSet<MorentCarReview> Reviews => Set<MorentCarReview>();
  // public DbSet<MorentFavorite> Favorites => Set<MorentFavorite>();
  // public DbSet<MorentImage> Images => Set<MorentImage>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new MorentUserConfiguration());
    // modelBuilder.ApplyConfigurationsFromAssembly(typeof(MorentDbContext).Assembly);
  }
}