namespace Morent.Application.Features.Car;

public class CreateCarCommandHandler : ICommandHandler<CreateCarCommand, Guid>
{
  private readonly ICarRepository _carRepository;
  // private readonly IUnitOfWork _unitOfWork;
  private readonly ICurrentUserService _currentUserService;

  public CreateCarCommandHandler(
      ICarRepository carRepository,
      // IUnitOfWork unitOfWork,
      ICurrentUserService currentUserService)
  {
    _carRepository = carRepository;
    // _unitOfWork = unitOfWork;
    _currentUserService = currentUserService;
  }

  public async Task<Guid> Handle(CreateCarCommand command, CancellationToken cancellationToken)
  {
    // Ensure only admin can create cars
    if (_currentUserService.Role != MorentUserRole.Admin)
      throw new UnauthorizedAccessException("Only administrators can create cars");

    var carId = Guid.NewGuid();
    var pricePerDay = new Money(command.PricePerDay, command.Currency);

    var location = new Location(
        command.Location.Address,
        command.Location.City,
        command.Location.State,
        command.Location.ZipCode,
        command.Location.Country,
        command.Location.Latitude,
        command.Location.Longitude);

    var car = new MorentCar(
        command.Brand,
        command.Model,
        command.Year,
        command.LicensePlate,
        command.Capacity,
        command.FuelType,
        pricePerDay,
        location
        );

    // Add images if provided
    if (command.Images != null)
    {
      foreach (var imageUrl in command.Images)
      {
        car.AddImage(imageUrl);
      }
    }

    await _carRepository.AddAsync(car, cancellationToken);
    await _carRepository.SaveChangesAsync();
    // await _unitOfWork.SaveChangesAsync(cancellationToken);

    return carId;
  }
}