using Microsoft.EntityFrameworkCore;
using Morent.Core.Entities;

namespace Morent.Core.Interfaces;

public interface IMorentDbContext
{
  DbSet<MorentUser> Users { get; }
  DbSet<MorentCar> Cars { get; }
  DbSet<MorentCarModel> CarModels { get; }
  DbSet<MorentCarType> CarTypes { get; }
  DbSet<MorentLocation> Locations { get; }
  DbSet<MorentRental> Rentals { get; }
  DbSet<MorentRentalDetail> RentalDetails { get; }
  DbSet<MorentPayment> Payments { get; }
  DbSet<MorentReview> Reviews { get; }
  DbSet<MorentFavorite> Favorites { get; }
  DbSet<MorentImage> Images { get; }

  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}