using Morent.Infrastructure.Data.Configs;

namespace Morent.Infrastructure.Data;

public class MorentDbContext : DbContext
{
  public MorentDbContext(DbContextOptions<MorentDbContext> options)
      : base(options)
  {
  }

  public DbSet<MorentUser> Users => Set<MorentUser>();
  public DbSet<MorentCar> Cars => Set<MorentCar>();
  public DbSet<MorentRental> Rentals => Set<MorentRental>();
  public DbSet<MorentReview> Reviews => Set<MorentReview>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new MorentUserConfiguration());
    // modelBuilder.ApplyConfigurationsFromAssembly(typeof(MorentDbContext).Assembly);
  }
}