using Morent.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Morent.Infrastructure.Services;

public class ImageService : IImageService
{
  private readonly IWebHostEnvironment _environment;
  private readonly IConfiguration _configuration;
  private readonly string _uploadPath;

  public ImageService(IWebHostEnvironment environment, IConfiguration configuration)
  {
    _environment = environment;
    _configuration = configuration;
    _uploadPath = Path.Combine(_environment.WebRootPath, "uploads");

    // Ensure directory exists
    if (!Directory.Exists(_uploadPath))
    {
      Directory.CreateDirectory(_uploadPath);
    }
  }

  public async Task<string> UploadImageAsync(IFormFile file, string folder)
  {
    if (file == null || file.Length == 0)
    {
      throw new ArgumentException("No file was provided");
    }

    // Check file extension
    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    if (!IsValidImageExtension(extension))
    {
      throw new ArgumentException("Invalid image format");
    }

    // Create folder if it doesn't exist
    var folderPath = Path.Combine(_uploadPath, folder);
    if (!Directory.Exists(folderPath))
    {
      Directory.CreateDirectory(folderPath);
    }

    // Generate unique filename
    var fileName = $"{Guid.NewGuid()}{extension}";
    var filePath = Path.Combine(folderPath, fileName);

    // Save file
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    // Return relative path
    return $"/uploads/{folder}/{fileName}";
  }

  public bool IsExternalImageUrl(string imageUrl)
  {
    return Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) &&
          (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
  }

  public bool IsValidImageUrl(string imageUrl)
  {
    // For local files, check if they exist in uploads folder
    if (!IsExternalImageUrl(imageUrl))
    {
      var localPath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
      return File.Exists(localPath);
    }

    // For external URLs, we trust that they're valid
    // In a production app, we might validate them more thoroughly
    return true;
  }

  private bool IsValidImageExtension(string extension)
  {
    var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
    return Array.IndexOf(validExtensions, extension) >= 0;
  }
}