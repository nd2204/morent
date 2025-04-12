using System;

using Morent.Core.Entities;
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
  public DbSet<MorentCarModel> CarModels => Set<MorentCarModel>();
  public DbSet<MorentCarType> CarTypes => Set<MorentCarType>();
  public DbSet<MorentRental> Rentals => Set<MorentRental>();
  public DbSet<MorentRentalDetail> RentalDetails => Set<MorentRentalDetail>();
  public DbSet<MorentPayment> Payments => Set<MorentPayment>();
  public DbSet<MorentReview> Reviews => Set<MorentReview>();
  public DbSet<MorentFavorite> Favorites => Set<MorentFavorite>();
  public DbSet<MorentImage> Images => Set<MorentImage>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MorentDbContext).Assembly);
  }
}