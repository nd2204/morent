namespace Morent.Application.Repositories;

public interface ICarRepository : IRepository<MorentCar>
{
  Task<IEnumerable<MorentCar>> GetAvailableCarsAsync(DateTime start, DateTime end, LocationDto nearLocation, int? minCapacity = null, CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentCar>> GetCarsByBrandAsync(string brand, CancellationToken cancellationToken = default);
  Task<MorentCar> GetCarWithRentalsAsync(Guid id, CancellationToken cancellationToken = default);
  Task<MorentCar> GetCarWithReviewsAsync(Guid id, CancellationToken cancellationToken = default);
}
