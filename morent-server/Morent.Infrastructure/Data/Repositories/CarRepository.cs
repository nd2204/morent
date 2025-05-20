using Morent.Application.Features.Car.DTOs;
using Morent.Application.Repositories;

namespace Morent.Infrastructure.Data.Repositories;

public class CarRepository : EFRepository<MorentCar>, ICarRepository
{
    private readonly MorentDbContext _context;

    public CarRepository(MorentDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MorentCar>> GetAvailableCarsAsync(
        DateTime? start,
        DateTime? end,
        LocationDto? nearLocation,
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
            query = query.Where(c => c.CarModel.SeatCapacity >= minCapacity.Value);
        }

        return await query
            .Include(c => c.CarModel)
            .Include(c => c.Images)
            .ToListAsync(cancellationToken);
    }

    public async Task<MorentCar?> GetCarWithRentalsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .Include(c => c.Images)
            .Include(c => c.Rentals)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<MorentCar?> GetCarWithReviewsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .Include(c => c.Images)
            .Include(c => c.Reviews)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MorentCar>> GetCarsByBrandAsync(string brand, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .Include(c => c.Images)
            .Where(c => c.CarModel.Brand.ToLower() == brand.ToLower())
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MorentCar>> GetCarsByModelAsync(string modelName, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .Include(c => c.Images)
            .Where(c => c.CarModel.ModelName.ToLower() == modelName.ToLower())
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MorentCarModel>> GetCarModelsByQuery(string? brand, string? modelName, int? year, CancellationToken cancellationToken = default)
    {
        // Start with a base query
        var query = _context.CarModels.AsQueryable();

        // Apply filters progressively
        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(m => m.Brand.ToLower() == brand.ToLower());

        if (!string.IsNullOrWhiteSpace(modelName))
            query = query.Where(m => m.ModelName.ToLower() == modelName.ToLower());

        if (year.HasValue)
            query = query.Where(m => m.Year == year.Value);

        // Execute the query and return results
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<MorentCar?> GetCarWithImagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Images)
            .Where(c => c.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<MorentRental>> GetActiveRentalsForCarAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Where(r => r.CarId == carId)
            .ToListAsync();
    }
}