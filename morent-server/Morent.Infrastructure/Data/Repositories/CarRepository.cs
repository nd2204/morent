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
        CarLocationDto? nearLocation,
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

        return await query.Include(c => c.CarModel).ToListAsync(cancellationToken);
    }

    public async Task<MorentCar?> GetCarWithRentalsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .Include(c => c.Rentals)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<MorentCar?> GetCarWithReviewsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .Include(c => c.Reviews)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MorentCar>> GetCarsByBrandAsync(string brand, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .Where(c => c.CarModel.Brand.ToLower() == brand.ToLower())
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MorentCar>> GetCarsByModelAsync(string brand, string modelName, CancellationToken cancellationToken = default)
    {
        var cars = (await GetCarsByBrandAsync(brand, cancellationToken)).AsQueryable();
        cars = cars.Where(
            c => c.CarModel.ModelName.ToLower() == modelName.ToLower());
        return cars;
    }

    public async Task<IEnumerable<MorentCarModel>> GetCarModelsByQuery(string brand, string modelName, int? year, CancellationToken cancellationToken = default)
    {
        return await _context.CarModels
            .Where(m => m.Brand.ToLower() == brand.ToLower() &&
                        m.ModelName.ToLower() == modelName &&
                        year == null ? m.Year == year : true)
            .ToListAsync();
    }
}