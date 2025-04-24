namespace Morent.Application.Interfaces;

public interface ICarImageService
{
  Task<IEnumerable<CarImageDto>> GetCarImagesAsync(Guid carId);
  Task<CarImageDto> SetPrimaryImageAsync(Guid carId, Guid imageId);
  Task<bool> ReorderCarImagesAsync(Guid carId, List<CarImageOrderItem> newOrder);
  Task<CarImageDto> AddCarImageAsync(Guid carId, UploadCarImageRequest imageUpload);
  Task<CarImageDto> AddCarImageAsync(Guid carId, ImageUploadRequest imageUpload, bool isPrimary);
  Task<bool> DeleteCarImageAsync(Guid carId, Guid imageId);
}
