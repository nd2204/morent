using Morent.Application.Extensions;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Interfaces;
using Morent.Application.Repositories;
using Morent.Core.MediaAggregate;

namespace Morent.Infrastructure.Services;

public class CarImageService : ICarImageService
{
  private readonly ICarRepository _carRepository;
  private readonly IRepository<MorentImage> _imageRepository;
  private readonly IImageService _imageService;
  private readonly IUnitOfWork _unitOfWork;

  public CarImageService(
      ICarRepository carRepository,
      IRepository<MorentImage> imageRepository,
      IImageService imageService,
      IUnitOfWork unitOfWork)
  {
    _carRepository = carRepository;
    _imageRepository = imageRepository;
    _imageService = imageService;
    _unitOfWork = unitOfWork;
  }

  public async Task<Result<IEnumerable<CarImageDto>>> GetCarImagesAsync(Guid carId, CancellationToken cancellationToken = default)
  {
    var car = await _carRepository.GetCarWithImagesAsync(carId);
    if (car == null)
    {
      return Result.NotFound($"Car with ID {carId} not found.");
    }

    var result = new List<CarImageDto>();

    foreach (var carImage in car.Images.OrderBy(i => i.DisplayOrder))
    {
      var image = await _imageRepository.GetByIdAsync(carImage.ImageId);
      if (image != null)
      {
        result.Add(new CarImageDto
        {
          ImageId = carImage.ImageId,
          IsPrimary = carImage.IsPrimary,
          DisplayOrder = carImage.DisplayOrder,
          Url = (await _imageService.GetImageByIdAsync(carImage.ImageId)).Value.Url
        });
      }
    }

    return Result.Success(result as IEnumerable<CarImageDto>);
  }

  public async Task<Result<CarImageDto>> AddCarImageAsync(Guid carId, ImageUploadRequest request, bool isPrimary, CancellationToken cancellationToken = default)
  {
    await _unitOfWork.BeginTransactionAsync();

    // First, upload the image
    var uploadResult = await _imageService.UploadImageAsync(request);
    if (!uploadResult.IsSuccess)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.Error($"Failed to upload image: {string.Join(", ", uploadResult.Errors)}");
    }

    return await AssignCarImageAsync(carId, uploadResult.Value.ImageId, isPrimary);
  }

  public async Task<Result<CarImageDto>> AddCarImageAsync(Guid carId, UploadCarImageRequest request, CancellationToken cancellationToken = default)
  {
    using var stream = request.Image.OpenReadStream();
    var imageUploadRequest = new ImageUploadRequest
    {
      ImageData = stream,
      FileName = request.Image.FileName,
      ContentType = request.Image.ContentType
    };
    return await AddCarImageAsync(carId, imageUploadRequest, request.SetAsPrimary);
  }

  public async Task<Result> DeleteCarImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default)
  {
    await _unitOfWork.BeginTransactionAsync();

    // Get the car
    var car = await _carRepository.GetByIdAsync(carId);
    if (car == null)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.NotFound($"Car with ID {carId} not found.");
    }

    // Check if image exists
    var carImage = car.Images.FirstOrDefault(i => i.ImageId == imageId);
    if (carImage == null)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.NotFound($"Image with ID {imageId} not found.");
    }

    // Remove from car
    car.RemoveImage(imageId);

    // Update car
    await _carRepository.UpdateAsync(car);

    // Delete image
    await _imageService.DeleteImageAsync(imageId);
    await _unitOfWork.CommitTransactionAsync();

    return Result.Success();
  }

  public async Task<Result<CarImageDto>> SetPrimaryImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default)
  {
    await _unitOfWork.BeginTransactionAsync();

    // Get the car
    var car = await _carRepository.GetByIdAsync(carId);
    if (car == null)
    {
      return CarNotFoundResult(carId);
    }

    // Set primary image
    car.SetPrimaryImage(imageId);

    // Update car
    await _carRepository.UpdateAsync(car);

    // Get updated car image
    var carImage = car.Images.First(i => i.ImageId == imageId);
    var imageUrl = _imageService.GetImageByIdAsync(imageId).Result.Value.Url;

    await _unitOfWork.CommitTransactionAsync();

    // Return result
    return Result.Success(new CarImageDto
    {
      ImageId = carImage.ImageId,
      IsPrimary = carImage.IsPrimary,
      DisplayOrder = carImage.DisplayOrder,
      Url = imageUrl
    });
  }

  public async Task<Result> ReorderCarImagesAsync(Guid carId, List<CarImageOrderItem> newOrder, CancellationToken cancellationToken = default)
  {
    if (newOrder == null || !newOrder.Any())
    {
      return Result.Invalid(new ValidationError("No reordering information provided"));
    }

    await _unitOfWork.BeginTransactionAsync();

    // Get the car
    var car = await _carRepository.GetByIdAsync(carId);
    if (car == null)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return CarNotFoundResult(carId);
    }

    // Validate order items
    foreach (var item in newOrder)
    {
      if (!car.Images.Any(i => i.ImageId == item.ImageId))
      {
        await _unitOfWork.RollbackTransactionAsync();
        return Result.Error($"Images with ID {item.ImageId} does not belong to this car");
      }
    }

    // Update order for each image
    foreach (var item in newOrder)
    {
      car.ReorderImage(item.ImageId, item.NewOrder);
    }

    // Update car
    await _carRepository.UpdateAsync(car);

    await _unitOfWork.CommitTransactionAsync();
    return Result.Success();
  }

  public async Task<Result<CarImageDto>> AssignCarImageAsync(Guid carId, Guid imageId, bool isPrimary, CancellationToken cancellationToken = default)
  {
    await _unitOfWork.BeginTransactionAsync();

    // Get the car with fresh tracking context
    var car = await _carRepository.GetByIdAsync(carId);
    if (car == null)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return CarNotFoundResult(carId);
    }

    // Get the image
    var image = await _imageRepository.GetByIdAsync(imageId);
    if (image == null)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.NotFound($"Image with ID {imageId} not found");
    }

    // Check if the image is already assigned to this car
    var existingCarImage = car.Images.FirstOrDefault(ci => ci.ImageId == imageId);
    if (existingCarImage != null)
    {
      // If the image is already assigned, just update the isPrimary flag if needed
      if (existingCarImage.IsPrimary != isPrimary)
      {
        existingCarImage.SetAsPrimary(isPrimary);
        await _carRepository.UpdateAsync(car);
      }

      await _unitOfWork.SaveChangesAsync();
      await _unitOfWork.CommitTransactionAsync();

      var result = existingCarImage.ToDto();
      result.Url = (await _imageService.GetImageByIdAsync(imageId)).Value.Url;
      return Result.Success(result);
    }

    // Add image to car
    var carImage = car.AddImage(image, isPrimary);

    await _carRepository.UpdateAsync(car);
    await _unitOfWork.SaveChangesAsync();
    await _unitOfWork.CommitTransactionAsync();

    var carImageResult = carImage.ToDto();
    carImageResult.Url = (await _imageService.GetImageByIdAsync(imageId)).Value.Url;
    return Result.Success(carImageResult);
  }

  private Result CarNotFoundResult(Guid carId) => Result.NotFound($"Car with ID {carId} not found.");
}