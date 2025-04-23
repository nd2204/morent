namespace Morent.Application.Repositories;

public interface IReviewRepository : IRepository<MorentReview>
{
  Task<IEnumerable<MorentReview>> GetReviewsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentReview>> GetReviewsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
  Task<double> GetCarAverageRatingAsync(Guid carId, CancellationToken cancellationToken = default);
  Task<bool> ExistsByUserAndCarAsync(Guid userId, Guid carId, CancellationToken cancellationToken = default);
}
