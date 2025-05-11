using Microsoft.EntityFrameworkCore;
using Morent.Application.Repositories;
using Morent.Core.MorentRentalAggregate;

namespace Morent.Infrastructure.Data.Repositories;

public class RentalRepository : EFRepository<MorentRental>, IRentalRepository
{
    private readonly MorentDbContext _context;

    public RentalRepository(MorentDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MorentRental>> GetRentalsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Include(r => r.Payment)
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MorentRental>> GetRentalsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Include(r => r.Payment)
            .Where(r => r.CarId == carId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MorentRental>> GetActiveRentalsAsync(CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.UtcNow;
        return await _context.Rentals
            .Include(r => r.Payment)
            .Where(r => r.IsActive())
            .ToListAsync(cancellationToken);
    }

    public async Task<MorentRental?> GetRentalByUserAndCarAsync(Guid userId, Guid carId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Include(r => r.Payment)
            .FirstOrDefaultAsync(r => r.CarId == carId && r.UserId == userId);
    }
}