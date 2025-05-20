using System;
using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Car.Commands;

public class UpdateCarCommand : ICommand<Result>
{
  public Guid Id { get; set; }
  public decimal PricePerDay { get; set; }
  public string Currency { get; set; } = null!;
  public bool IsAvailable { get; set; }
  public LocationDto Location { get; set; } = null!;
  public List<UploadCarImageRequest> ImagesToAdd { get; set; } = null!;
  public List<Guid> ImagesToDelete { get; set; } = null!;
}
