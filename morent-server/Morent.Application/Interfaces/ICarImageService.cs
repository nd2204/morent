namespace Morent.Application.Interfaces;

public interface ICarImageService
{
  Task<IEnumerable<CarImageDto>> GetCarImagesAsync(Guid carId, CancellationToken cancellationToken = default);
  Task<CarImageDto> SetPrimaryImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default);
  Task<bool> ReorderCarImagesAsync(Guid carId, List<CarImageOrderItem> newOrder, CancellationToken cancellationToken = default);
  Task<CarImageDto> AddCarImageAsync(Guid carId, UploadCarImageRequest imageUpload, CancellationToken cancellationToken = default);
  Task<CarImageDto> AddCarImageAsync(Guid carId, ImageUploadRequest imageUpload, bool isPrimary, CancellationToken cancellationToken = default);
  Task<CarImageDto> AssignCarImageAsync(Guid carId, Guid imageId, bool isPrimary, CancellationToken cancellationToken = default);
  Task<bool> DeleteCarImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default);
}
