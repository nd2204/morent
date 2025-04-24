using Microsoft.EntityFrameworkCore;
using Morent.Application.Repositories;
using Morent.Core.MorentReviewAggregate;

namespace Morent.Infrastructure.Data.Repositories;

public class ReviewRepository : EFRepository<MorentReview>, IReviewRepository
{
    private readonly MorentDbContext _context;

    public ReviewRepository(MorentDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MorentReview>> GetReviewsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Where(r => r.CarId == carId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MorentReview>> GetReviewsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUserAndCarAsync(Guid userId, Guid carId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .AnyAsync(r => r.UserId == userId && r.CarId == carId, cancellationToken);
    }

    public async Task<double> GetCarAverageRatingAsync(Guid carId, CancellationToken cancellationToken)
    {
        return await _context.Reviews
            .Where(r => r.CarId == carId)
            .AverageAsync(r => r.Rating, cancellationToken);
    }
}