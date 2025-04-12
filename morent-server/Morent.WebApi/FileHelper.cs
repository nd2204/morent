using Microsoft.AspNetCore.Http;

namespace Morent.Api.Helpers;

public static class FileHelper
{
  private const string DefaultImagePath = "wwwroot/images";

  public static async Task<(string fileName, string filePath)> SaveImageFileAsync(
      IFormFile file,
      string? customPath = null)
  {
    if (file == null || file.Length == 0)
      throw new ArgumentException("No file provided");

    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
    var uploadPath = Path.Combine(customPath ?? DefaultImagePath, fileName);

    // Ensure directory exists
    var directory = Path.GetDirectoryName(uploadPath);
    if (!Directory.Exists(directory))
      Directory.CreateDirectory(directory!);

    using (var stream = new FileStream(uploadPath, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    return (fileName, uploadPath);
  }

  public static void DeleteImageFile(string fileName, string? customPath = null)
  {
    var imagePath = (customPath ?? DefaultImagePath) + fileName;
    DeleteFile(imagePath);
  }

  public static void DeleteFile(string filePath)
  {
    if (!File.Exists(filePath))
      throw new FileNotFoundException("FILE NOT FOUND", filePath);
    File.Delete(filePath);
  }

  public static string GetFileUrl(string fileName, string? basePath = null)
  {
    return $"/{basePath ?? "images"}/{fileName}";
  }
}