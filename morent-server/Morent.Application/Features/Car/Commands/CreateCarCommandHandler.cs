using Morent.Application.Extensions;

namespace Morent.Application.Features.Car.Commands;

public class CreateCarCommandHandler : ICommandHandler<CreateCarCommand, Result<Guid>>
{
  private readonly ICarRepository _carRepository;
  private readonly IUnitOfWork _unitOfWork;
  private readonly ICarImageService _carImageService;

  public CreateCarCommandHandler(
      IUnitOfWork unitOfWork,
      ICarRepository carRepository,
      ICarImageService carImageService
      )
  {
    _carRepository = carRepository;
    _unitOfWork = unitOfWork;
    _carImageService = carImageService;
  }

  public async Task<Result<Guid>> Handle(CreateCarCommand command, CancellationToken cancellationToken)
  {
    var carId = Guid.NewGuid();
    var pricePerDayResult = Money.Create(command.PricePerDay, command.Currency);

    if (pricePerDayResult.IsInvalid())
      return Result.Invalid(pricePerDayResult.ValidationErrors);

    var location = command.Location;

    var car = new MorentCar(
        command.ModelId,
        command.LicensePlate,
        pricePerDayResult.Value,
        location.ToEntity()
        );

    // Add images if provided
    if (command.Images != null)
    {
      foreach (var request in command.Images)
      {
        await _carImageService.AddCarImageAsync(car.Id, request);
      }
    }

    await _carRepository.AddAsync(car, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Result.Success(carId);
  }
}