using System;

namespace Morent.Application.Repositories;

public interface IRentalRepository : IRepository<MorentRental>
{
  Task<IEnumerable<MorentRental>> GetRentalsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentRental>> GetActiveRentalsAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentRental>> GetRentalsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default);
  Task<MorentRental?> GetRentalByUserAndCarAsync(Guid userId, Guid carId, CancellationToken cancellationToken = default);
}