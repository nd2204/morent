using Microsoft.AspNetCore.Hosting;
using Morent.Application.Interfaces;
using Morent.Core.MediaAggregate;

namespace Morent.Infrastructure.Services;

public class LocalFileSystemStorage : IImageStorage
{
  private readonly IWebHostEnvironment _env;
  private readonly ILogger<LocalFileSystemStorage> _logger;
  private readonly string _basePath;
  private readonly string _baseUrl;

  public LocalFileSystemStorage(
    IWebHostEnvironment environment,
    ILogger<LocalFileSystemStorage> logger,
    IConfiguration configuration)
  {
    _env = environment;
    _logger = logger;
    _basePath = Path.Combine(_env.WebRootPath, "uploads");
    _baseUrl = configuration["Storage:BaseUrl"] ?? "/uploads";

    // Ensure directory exists
    if (!Directory.Exists(_basePath))
    {
      Directory.CreateDirectory(_basePath);
    }
  }

  public Task<bool> DeleteImageAsync(string path)
  {
    throw new NotImplementedException();
  }

  public async Task<ImageStorageResult> StoreImageAsync(Stream imageStream, string fileName, string contentType)
  {
    try
    {
      // Generate unique file name with timestamp prefix
      string safeFileName = GetSafeFileName(fileName);
      string fileExtension = Path.GetExtension(safeFileName);
      string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
      string uniqueFileName = $"{timestamp}_{Guid.NewGuid()}{fileExtension}";

      // Determine folder based on content type
      string folderPath = DetermineFolderPath(contentType);
      string fullFolderPath = Path.Combine(_basePath, folderPath);

      // Ensure folder exists
      if (!Directory.Exists(fullFolderPath))
      {
        Directory.CreateDirectory(fullFolderPath);
      }

      // Build full path
      string filePath = Path.Combine(folderPath, uniqueFileName);
      string fullPath = Path.Combine(_basePath, filePath);

      // Write file
      using (var fileStream = new FileStream(fullPath, FileMode.Create))
      {
        await imageStream.CopyToAsync(fileStream);
      }

      // Return success
      return new ImageStorageResult
      {
        Success = true,
        Path = filePath.Replace('\\', '/'),
        Url = GetImageUrl(filePath)
      };
    }
    catch (Exception ex)
    {
      return new ImageStorageResult
      {
        Success = false,
        Error = $"Failed to store image: {ex.Message}"
      };
    }
  }

  public async Task CleanupOrphanedImages(IRepository<MorentImage> images)
  {
    try
    {
      Console.WriteLine("Starting orphaned image cleanup...");

      // Get the uploads directory path
      string uploadsDirectory = Path.Combine(_env.WebRootPath, "uploads");
      if (!Directory.Exists(uploadsDirectory))
      {
        Console.WriteLine("Uploads directory does not exist. Nothing to clean up.");
        return;
      }

      // Get all files in the uploads directory
      var filesOnDisk = Directory.GetFiles(uploadsDirectory, "*.*", SearchOption.AllDirectories)
          .Select(file => Path.GetRelativePath(uploadsDirectory, file))
          .ToList();

      Console.WriteLine($"Found {filesOnDisk.Count} files on disk in uploads directory.");

      // Get all image filenames from the database
      var imagesInDb = (await images.ListAsync()).AsQueryable()
          .Select(img => img.Path)
          .ToList();

      Console.WriteLine($"Found {imagesInDb} images registered in database.");

      // Find files that exist on disk but not in the database
      var orphanedFiles = filesOnDisk
          .Where(file => !imagesInDb.Contains(file))
          .ToList();

      Console.WriteLine($"Found {orphanedFiles.Count} orphaned image files to delete.");

      // Delete orphaned files
      int deletedCount = 0;
      int errorCount = 0;

      foreach (var fileName in orphanedFiles)
      {
        try
        {
          Console.WriteLine($"{fileName}");
          string filePath = Path.Combine(uploadsDirectory, fileName);
          if (File.Exists(filePath))
          {
            File.Delete(filePath);
            deletedCount++;
            Console.WriteLine($"Deleted orphaned file: {fileName}");
          }
        }
        catch (Exception ex)
        {
          errorCount++;
          Console.WriteLine($"Error deleting file {fileName}: {ex.Message}");
        }
      }

      Console.WriteLine($"Cleanup completed. Deleted {deletedCount} files. Errors: {errorCount}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error during image cleanup: {ex.Message}");
    }
  }


  public string GetImageUrl(string path)
  {
    return $"{_baseUrl}/{path.Replace('\\', '/')}";
  }

  private string GetSafeFileName(string fileName)
  {
    // Replace invalid characters
    foreach (var c in Path.GetInvalidFileNameChars())
    {
      fileName = fileName.Replace(c, '_');
    }

    return fileName;
  }

  private string DetermineFolderPath(string contentType)
  {
    // Organize files by content type and year/month
    string yearMonth = DateTime.UtcNow.ToString("yyyy/MM");

    if (contentType.StartsWith("image/"))
    {
      return Path.Combine("images", yearMonth);
    }

    // Default folder
    return Path.Combine("misc", yearMonth);
  }
}
