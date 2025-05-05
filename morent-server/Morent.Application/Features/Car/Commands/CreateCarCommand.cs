namespace Morent.Application.Features.Car.Commands;

public class CreateCarCommand : ICommand<Result<Guid>>
{
  public int Year { get; set; }
  public Guid ModelId { get; set; }
  public string LicensePlate { get; set; } = null!;
  public decimal PricePerDay { get; set; }
  public string Currency { get; set; } = null!;
  public CarLocationDto Location { get; set; } = null!;
  public List<UploadCarImageRequest> Images { get; set; } = null!;
}