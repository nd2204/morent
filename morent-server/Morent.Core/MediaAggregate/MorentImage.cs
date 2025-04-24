using System;
using Morent.Core.Exceptions;

namespace Morent.Core.MediaAggregate;

public class MorentImage : EntityBase<Guid>, IAggregateRoot
{
  public string FileName { get; private set; }
  public string ContentType { get; private set; }
  public string Path { get; private set; }
  public DateTime UploadedAt { get; private set; }
  public long Size { get; private set; }

  public MorentImage(string fileName, string contentType, string path, long size)
  {
    Id = Guid.NewGuid();
    FileName = fileName;
    ContentType = contentType;
    Path = path;
    UploadedAt = DateTime.UtcNow;
    Size = size;
  }

  // Factory method to create from upload data
  public static MorentImage Create(string originalFileName, string contentType, string storagePath, long size)
  {
    // Validate content type
    if (!IsValidContentType(contentType))
      throw new DomainException($"Invalid content type: {contentType}. Only JPEG and PNG are supported.");

    // Validate file size
    if (size > 5 * 1024 * 1024) // 5MB
      throw new DomainException($"File size exceeds maximum allowed (5MB)");

    return new MorentImage(originalFileName, contentType, storagePath, size);
  }

  private static bool IsValidContentType(string contentType)
  {
    return contentType == "image/jpeg" || contentType == "image/png";
  }
}