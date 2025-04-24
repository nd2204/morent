using System;

namespace Morent.Application.Features.Car;

public class UpdateCarCommand : ICommand<bool>
{
  public Guid Id { get; set; }
  public decimal PricePerDay { get; set; }
  public string Currency { get; set; } = null!;
  public bool IsAvailable { get; set; }
  public LocationDto Location { get; set; } = null!;
  public List<string> ImagesToAdd { get; set; } = null!;
  public List<string> ImagesToRemove { get; set; } = null!;
}
