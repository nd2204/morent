using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Repositories;

public interface ICarRepository : IRepository<MorentCar>
{
  Task<IEnumerable<MorentCarModel>> GetCarModelsByQuery(string brand, string modelName, int? year, CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentCar>> GetAvailableCarsAsync(DateTime? start, DateTime? end, CarLocationDto? nearLocation = null, int? minCapacity = null, CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentCar>> GetCarsByBrandAsync(string brand, CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentCar>> GetCarsByModelAsync(string brancd, string modelName, CancellationToken cancellationToken = default);
  Task<MorentCar?> GetCarWithRentalsAsync(Guid id, CancellationToken cancellationToken = default);
  Task<MorentCar?> GetCarWithReviewsAsync(Guid id, CancellationToken cancellationToken = default);
  Task<MorentCar?> GetCarWithImagesAsync(Guid id, CancellationToken cancellationToken = default);
  Task<IEnumerable<MorentRental>> GetActiveRentalsForCarAsync(Guid carId, CancellationToken cancellationToken = default);
}
