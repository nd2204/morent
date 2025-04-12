using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Morent.Core.Entities;

public class MorentImage
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string FileName { get; set; } = string.Empty;
  public string Url { get; set; } = string.Empty;
  public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

  // Car image (optional)
  public int? CarModelId { get; set; }
  [ForeignKey(nameof(CarModelId))]
  public MorentCarModel? CarModel { get; set; }

  // User profile image (optional)
  public Guid? UserId { get; set; }
  [ForeignKey(nameof(UserId))]
  public MorentUser? User { get; set; }
}