namespace Morent.Application.Interfaces;

public interface ICarImageService
{
  Task<Result<IEnumerable<CarImageDto>>> GetCarImagesAsync(Guid carId, CancellationToken cancellationToken = default);
  Task<Result<CarImageDto>> SetPrimaryImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default);
  Task<Result> ReorderCarImagesAsync(Guid carId, List<CarImageOrderItem> newOrder, CancellationToken cancellationToken = default);
  Task<Result<CarImageDto>> AddCarImageAsync(Guid carId, UploadCarImageRequest imageUpload, CancellationToken cancellationToken = default);
  Task<Result<CarImageDto>> AddCarImageAsync(Guid carId, ImageUploadRequest imageUpload, bool isPrimary, CancellationToken cancellationToken = default);
  Task<Result<CarImageDto>> AssignCarImageAsync(Guid carId, Guid imageId, bool isPrimary, CancellationToken cancellationToken = default);
  Task<Result> DeleteCarImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default);
}
