using Morent.Application.Features.Images.DTOs;
using Morent.Application.Interfaces;
using Morent.Core.MediaAggregate;

namespace Morent.Infrastructure.Services;
public class ImageService : IImageService
{
  private readonly IRepository<MorentImage> _imageRepository;
  private readonly IImageStorage _imageStorage;
  private readonly HttpClient _httpClient;
  private readonly IUnitOfWork _unitOfWork;

  public ImageService(
      IRepository<MorentImage> imageRepository,
      IImageStorage imageStorage,
      HttpClient httpClient,
      IUnitOfWork unitOfWork)
  {
    _imageRepository = imageRepository;
    _imageStorage = imageStorage;
    _unitOfWork = unitOfWork;
    _httpClient = httpClient;
  }

  public async Task<ImageUploadResponse> UploadImageAsync(ImageUploadRequest request)
  {
    // Validate image before processing
    var validationErrors = ValidateImage(request);
    if (validationErrors.Count > 0)
    {
      return ImageUploadResponse.Failed(validationErrors);
    }

    try
    {
      await _unitOfWork.BeginTransactionAsync();

      // Store image in storage
      var storageResult = await _imageStorage.StoreImageAsync(
          request.ImageData,
          request.FileName,
          request.ContentType);

      if (!storageResult.Success)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return ImageUploadResponse.Failed(new List<string> { storageResult.Error });
      }

      // Create image entity
      var image = MorentImage.Create(
          request.FileName,
          request.ContentType,
          storageResult.Path,
          request.ImageData.Length);

      // Save to database
      var savedImage = await _imageRepository.AddAsync(image);

      await _unitOfWork.CommitTransactionAsync();

      // Return successful result
      return ImageUploadResponse.Successful(
          savedImage.Id,
          storageResult.Path,
          _imageStorage.GetImageUrl(storageResult.Path));
    }
    catch (Exception ex)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return ImageUploadResponse.Failed(new List<string> { $"Error uploading image: {ex.Message}" });
    }
  }

  public async Task<bool> DeleteImageAsync(Guid imageId)
  {
    try
    {
      await _unitOfWork.BeginTransactionAsync();

      var image = await _imageRepository.GetByIdAsync(imageId);
      if (image == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
      }

      // Delete from storage
      var storageDeleteResult = await _imageStorage.DeleteImageAsync(image.Path);
      if (!storageDeleteResult)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
      }

      // Delete from database
      var imageToDelete = await _imageRepository.GetByIdAsync(imageId);
      if (imageToDelete == null) {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
      }

      var repoDeleteResult = await _imageRepository.DeleteAsync(imageToDelete);
      if (repoDeleteResult < 0)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
      }

      await _unitOfWork.CommitTransactionAsync();
      return true;
    }
    catch
    {
      await _unitOfWork.RollbackTransactionAsync();
      return false;
    }
  }

  public async Task<ImageResult> GetImageByIdAsync(Guid imageId)
  {
    var image = await _imageRepository.GetByIdAsync(imageId);
    if (image == null)
    {
      return null!;
    }

    return new ImageResult
    {
      Id = image.Id,
      FileName = image.FileName,
      ContentType = image.ContentType,
      Size = image.Size,
      UploadedAt = image.UploadedAt,
      Url = _imageStorage.GetImageUrl(image.Path)
    };
  }

  private List<string> ValidateImage(ImageUploadRequest request)
  {
    var errors = new List<string>();

    // Check content type
    if (request.ContentType != "image/jpeg" && request.ContentType != "image/png")
    {
      errors.Add("Only JPEG and PNG image formats are supported");
    }

    // Check file size (5MB max)
    const long maxFileSize = 5 * 1024 * 1024;
    if (request.ImageData.Length > maxFileSize)
    {
      errors.Add($"Image size exceeds the maximum allowed size of 5MB");
    }

    return errors;
  }

  private async Task<ImageUploadRequest?> FetchImageAsUploadRequest(string imageUrl)
  {
    var response = await _httpClient.GetAsync(imageUrl);
    if (!response.IsSuccessStatusCode)
      return null;

    var contentType = response.Content.Headers.ContentType?.MediaType;

    if (string.IsNullOrWhiteSpace(contentType))
      return null;

    if(!contentType.StartsWith("image/"))
      return null;

    if (!response.IsSuccessStatusCode)
      throw new Exception($"Failed to fetch image. StatusCode: {response.StatusCode}");
    

    var image = await response.Content.ReadAsStreamAsync();
    return new ImageUploadRequest
    {
      ImageData = image,
      ContentType = contentType,
      FileName = await DetermineImageFileNameAsync(imageUrl)
    };
  }


  public async Task<string> DetermineImageFileNameAsync(string imageUrl)
  {
    var response = await _httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);

    if (!response.IsSuccessStatusCode)
      throw new Exception("Failed to fetch image");

    // 1. Try to get from Content-Disposition
    var fileName = GetFileNameFromContentDisposition(response);

    if (string.IsNullOrEmpty(fileName))
    {
      // 2. Fallback to URL filename
      fileName = GetFileNameFromUrl(imageUrl);
    }

    return fileName;
  }

  private string? GetFileNameFromContentDisposition(HttpResponseMessage response)
  {
    if (response.Content.Headers.ContentDisposition?.FileName != null)
    {
      return response.Content.Headers.ContentDisposition.FileName.Trim('\"');
    }
    return null;
  }

  private string GetFileNameFromUrl(string url)
  {
    return Path.GetFileName(new Uri(url).AbsolutePath);
  }
}
