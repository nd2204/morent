using Microsoft.EntityFrameworkCore;
using Morent.Application.DTOs;
using Morent.Application.Repositories;
using Morent.Core.MorentCarAggregate;

namespace Morent.Infrastructure.Data.Repositories;

public class CarRepository : EFRepository<MorentCar>, ICarRepository
{
    private readonly MorentDbContext _context;

    public CarRepository(MorentDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MorentCar>> GetAvailableCarsAsync(
        DateTime start, 
        DateTime end, 
        LocationDto nearLocation, 
        int? minCapacity = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Cars.AsQueryable();

        // Filter by availability
        query = query.Where(c => c.IsAvailable);

        // Filter by location if provided
        if (nearLocation != null)
        {
            // Implement location-based filtering logic here
            // This could involve calculating distance from the provided location
        }

        // Filter by capacity if provided
        if (minCapacity.HasValue)
        {
            query = query.Where(c => c.Capacity >= minCapacity.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MorentCar>> GetCarsByBrandAsync(string brand, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Where(c => c.Brand.ToLower() == brand.ToLower())
            .ToListAsync(cancellationToken);
    }

    public async Task<MorentCar?> GetCarWithRentalsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Rentals)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<MorentCar?> GetCarWithReviewsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Reviews)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}