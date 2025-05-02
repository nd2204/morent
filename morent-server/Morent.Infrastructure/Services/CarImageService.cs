using System;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Features.Images.DTOs;
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

  public async Task<IEnumerable<CarImageDto>> GetCarImagesAsync(Guid carId, CancellationToken cancellationToken = default)
  {
    var car = await _carRepository.GetCarWithImagesAsync(carId);
    if (car == null)
    {
      return Enumerable.Empty<CarImageDto>();
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
          Url = _imageService.GetImageByIdAsync(carImage.ImageId).Result.Url
        });
      }
    }

    return result;
  }

  public async Task<CarImageDto> AddCarImageAsync(Guid carId, ImageUploadRequest request, bool isPrimary, CancellationToken cancellationToken = default)
  {
    try
    {
      await _unitOfWork.BeginTransactionAsync();

      // First, upload the image
      var uploadResult = await _imageService.UploadImageAsync(request);
      if (!uploadResult.Success)
      {
        await _unitOfWork.RollbackTransactionAsync();
        throw new ApplicationException($"Failed to upload image: {string.Join(", ", uploadResult.Errors)}");
      }

      return await AssignCarImageAsync(carId, uploadResult.ImageId, isPrimary);
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollbackTransactionAsync();
      throw new ApplicationException($"Error adding car image: {ex.Message}", ex);
    }
  }

  public async Task<CarImageDto> AddCarImageAsync(Guid carId, UploadCarImageRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      using var stream = request.Image.OpenReadStream(); 
      var imageUploadRequest = new ImageUploadRequest {
        ImageData = stream,
        FileName = request.Image.FileName,
        ContentType = request.Image.ContentType
      };
      return await AddCarImageAsync(carId, imageUploadRequest, request.SetAsPrimary);
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollbackTransactionAsync();
      throw new ApplicationException($"Error adding car image: {ex.Message}", ex);
    }
  }

  public async Task<bool> DeleteCarImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default)
  {
    try
    {
      await _unitOfWork.BeginTransactionAsync();

      // Get the car
      var car = await _carRepository.GetByIdAsync(carId);
      if (car == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
      }

      // Check if image exists
      var carImage = car.Images.FirstOrDefault(i => i.ImageId == imageId);
      if (carImage == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
      }

      // Remove from car
      car.RemoveImage(imageId);

      // Update car
      await _carRepository.UpdateAsync(car);

      // Delete image
      await _imageService.DeleteImageAsync(imageId);

      await _unitOfWork.CommitTransactionAsync();
      return true;
    }
    catch
    {
      await _unitOfWork.RollbackTransactionAsync();
      return false;
    }
  }

  public async Task<CarImageDto> SetPrimaryImageAsync(Guid carId, Guid imageId, CancellationToken cancellationToken = default)
  {
    try
    {
      await _unitOfWork.BeginTransactionAsync();

      // Get the car
      var car = await _carRepository.GetByIdAsync(carId);
      if (car == null)
      {
        throw new ApplicationException($"Car with ID {carId} not found");
      }

      // Set primary image
      car.SetPrimaryImage(imageId);

      // Update car
      await _carRepository.UpdateAsync(car);

      // Get updated car image
      var carImage = car.Images.First(i => i.ImageId == imageId);
      var imageUrl = _imageService.GetImageByIdAsync(imageId).Result.Url;

      await _unitOfWork.CommitTransactionAsync();

      // Return result
      return new CarImageDto
      {
        ImageId = carImage.ImageId,
        IsPrimary = carImage.IsPrimary,
        DisplayOrder = carImage.DisplayOrder,
        Url = imageUrl
      };
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollbackTransactionAsync();
      throw new ApplicationException($"Error setting primary car image: {ex.Message}", ex);
    }
  }

  public async Task<bool> ReorderCarImagesAsync(Guid carId, List<CarImageOrderItem> newOrder, CancellationToken cancellationToken = default)
  {
    try
    {
      await _unitOfWork.BeginTransactionAsync();

      // Get the car
      var car = await _carRepository.GetByIdAsync(carId);
      if (car == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
      }

      // Validate order items
      foreach (var item in newOrder)
      {
        if (!car.Images.Any(i => i.ImageId == item.ImageId))
        {
          await _unitOfWork.RollbackTransactionAsync();
          return false;
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
      return true;
    }
    catch
    {
      await _unitOfWork.RollbackTransactionAsync();
      return false;
    }
  }

  public async Task<CarImageDto> AssignCarImageAsync(Guid carId, Guid imageId, bool isPrimary, CancellationToken cancellationToken = default)
  {
    try
    {
      await _unitOfWork.BeginTransactionAsync();

      // Get the car with fresh tracking context
      var car = await _carRepository.GetByIdAsync(carId);
      if (car == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        throw new ApplicationException($"Car with ID {carId} not found");
      }

      // Get the image
      var image = await _imageRepository.GetByIdAsync(imageId);
      if (image == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        throw new ApplicationException($"Image with ID {imageId} not found");
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

        return new CarImageDto
        {
          ImageId = existingCarImage.ImageId,
          IsPrimary = existingCarImage.IsPrimary,
          DisplayOrder = existingCarImage.DisplayOrder,
          Url = _imageService.GetImageByIdAsync(imageId).Result.Url
        };
      }

      // Add image to car
      var carImage = car.AddImage(image, isPrimary);

      // Update car with retry logic for concurrency issues
      bool success = false;
      int maxRetries = 3;
      int retryCount = 0;

      while (!success && retryCount < maxRetries)
      {
        try
        {
          await _carRepository.UpdateAsync(car);
          await _unitOfWork.SaveChangesAsync();
          success = true;
        }
        catch (DbUpdateConcurrencyException)
        {
          retryCount++;
          if (retryCount >= maxRetries)
          {
            throw;  // Re-throw if we've exhausted our retries
          }

          // Get a fresh copy of the car and try again
          await _unitOfWork.RollbackTransactionAsync();
          await _unitOfWork.BeginTransactionAsync();

          car = await _carRepository.GetByIdAsync(carId);
          if (car == null)
          {
            await _unitOfWork.RollbackTransactionAsync();
            throw new ApplicationException($"Car with ID {carId} not found on retry {retryCount}");
          }

          // Add the image again to the fresh car entity
          carImage = car.AddImage(image, isPrimary);
        }
      }

      await _unitOfWork.CommitTransactionAsync();

      // Return result
      return new CarImageDto
      {
        ImageId = carImage.ImageId,
        IsPrimary = carImage.IsPrimary,
        DisplayOrder = carImage.DisplayOrder,
        Url = _imageService.GetImageByIdAsync(imageId).Result.Url
      };
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollbackTransactionAsync();
      throw new ApplicationException($"Error setting primary car image: {ex.Message}", ex);
    }
  }
}