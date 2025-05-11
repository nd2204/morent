using Ardalis.Specification;
using Morent.Application.Interfaces;
using Morent.Core.MediaAggregate;
using Morent.Core.MediaAggregate.Specifications;

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

  public async Task<Result<ImageUploadResponse>> UploadImageAsync(ImageUploadRequest request)
  {
    // Validate image before processing
    var validationErrors = ValidateImage(request);
    if (validationErrors.Count > 0)
    {
      return Result.Invalid(validationErrors);
    }

    await _unitOfWork.BeginTransactionAsync();

    // Store image in storage
    var storageResult = await _imageStorage.StoreImageAsync(
        request.ImageData,
        request.FileName,
        request.ContentType);

    if (!storageResult.Success)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.Error(storageResult.Error);
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
    return Result.Success(new ImageUploadResponse(
        savedImage.Id,
        storageResult.Path,
        _imageStorage.GetImageUrl(storageResult.Path)));
  }

  public async Task<Result> DeleteImageAsync(Guid imageId)
  {
      await _unitOfWork.BeginTransactionAsync();

      var image = await _imageRepository.GetByIdAsync(imageId);
      if (image == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return Result.NotFound($"Image with ID {imageId} not found");
      }

      // Delete from storage
      var storageDeleteResult = await _imageStorage.DeleteImageAsync(image.Path);
      if (!storageDeleteResult)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return Result.Error("Error when deleting image from storage");
      }

      // Delete from database
      var imageToDelete = await _imageRepository.GetByIdAsync(imageId);
      if (imageToDelete == null)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return Result.Error($"Error when deleting image from database: Image with ID {imageId} not found.");
      }

      var repoDeleteResult = await _imageRepository.DeleteAsync(imageToDelete);
      if (repoDeleteResult < 0)
      {
        await _unitOfWork.RollbackTransactionAsync();
        return Result.Error($"Error when deleting image from database: negative row affected");
      }

      await _unitOfWork.CommitTransactionAsync();
      return Result.Success();
  }

  public async Task<Result<ImageDto>> GetImageByIdAsync(Guid imageId)
  {
    var image = await _imageRepository.GetByIdAsync(imageId);
    if (image == null)
    {
      return Result.NotFound($"Image with ID {imageId} not found.");
    }

    return Result.Success(new ImageDto
    {
      Id = image.Id,
      FileName = image.FileName,
      ContentType = image.ContentType,
      Size = image.Size,
      UploadedAt = image.UploadedAt,
      Url = _imageStorage.GetImageUrl(image.Path)
    });
  }

  private List<ValidationError> ValidateImage(ImageUploadRequest request)
  {
    var errors = new List<ValidationError>();

    // Check content type
    if (request.ContentType != "image/jpeg" && request.ContentType != "image/png")
    {
      errors.Add(new ValidationError("ContentType", "Only JPEG and PNG image formats are supported"));
    }

    // Check file size (5MB max)
    const long maxFileSize = 5 * 1024 * 1024;
    if (request.ImageData.Length > maxFileSize)
    {
      errors.Add(new ValidationError("ImageData", $"Image size exceeds the maximum allowed size of 5MB"));
    }

    return errors;
  }

  public async Task<Result<ImageUploadRequest>> FetchImageAsUploadRequest(string imageUrl)
  {
    var response = await _httpClient.GetAsync(imageUrl);
    if (!response.IsSuccessStatusCode)
      return Result.Error($"Failed to fetch image. StatusCode: {response.StatusCode}");

    var contentType = response.Content.Headers.ContentType?.MediaType;

    if (string.IsNullOrWhiteSpace(contentType))
      return Result.Invalid(new ValidationError("Content type must not be null or empty"));

    if (!contentType.StartsWith("image/"))
      return Result.Invalid(new ValidationError("Content type not start with image/"));

    var imageExt = contentType.Split("/")[1];

    var image = await response.Content.ReadAsStreamAsync();
    return Result.Success(new ImageUploadRequest
    {
      ImageData = image,
      ContentType = contentType,
      FileName = await DetermineImageFileNameAsync(imageUrl) + $".{imageExt}"
    });
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
      fileName = $"external-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid()}";
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

  // private string GetFileNameFromUrl(string url)
  // {
  //   return Path.GetFileName(new Uri(url).AbsolutePath);
  // }
 
  public async Task<Result<ImageDto>> GetPlaceHolderImageAsync()
  {
    var placeholderImageName = "placeholder.png";
    var image = await _imageRepository.FirstOrDefaultAsync(new ImageByFileNameSpec(placeholderImageName));
    if (image == null)
    {
      return Result.NotFound($"Image with FileName {placeholderImageName} not found.");
    }

    return Result.Success(new ImageDto
    {
      Id = image.Id,
      FileName = image.FileName,
      ContentType = image.ContentType,
      Size = image.Size,
      UploadedAt = image.UploadedAt,
      Url = _imageStorage.GetImageUrl(image.Path)
    });
  }

  public async Task<Result<ImageUploadResponse>> UploadImageAsync(string imageUrl)
  {
    Result<ImageUploadRequest> result = await FetchImageAsUploadRequest(imageUrl);
    if (result.IsError())
      return Result.Error(new ErrorList(result.Errors));

    if (result.IsInvalid())
      return Result.Invalid(result.ValidationErrors);
    
    return await UploadImageAsync(result.Value);
  }
}