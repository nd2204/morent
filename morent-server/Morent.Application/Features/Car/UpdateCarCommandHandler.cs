using System;

namespace Morent.Application.Features.Car;

public class UpdateCarCommandHandler : ICommandHandler<UpdateCarCommand, bool>
{
  private readonly ICarRepository _carRepository;
  // private readonly IUnitOfWork _unitOfWork;
  private readonly ICurrentUserService _currentUserService;

  public UpdateCarCommandHandler(
      ICarRepository carRepository,
      // IUnitOfWork unitOfWork,
      ICurrentUserService currentUserService)
  {
    _carRepository = carRepository;
    // _unitOfWork = unitOfWork;
    _currentUserService = currentUserService;
  }

  public async Task<bool> Handle(UpdateCarCommand command, CancellationToken cancellationToken)
  {
    // Ensure only admin can update cars
    if (_currentUserService.Role != MorentUserRole.Admin)
      throw new UnauthorizedAccessException("Only administrators can update cars");

    var car = await _carRepository.GetByIdAsync(command.Id, cancellationToken);
    if (car == null)
      throw new ApplicationException($"Car with ID {command.Id} not found");

    // Update price if provided
    if (command.PricePerDay > 0)
    {
      var newPrice = new Money(command.PricePerDay, command.Currency);
      car.UpdatePrice(newPrice);
    }

    // Update location if provided
    if (command.Location != null)
    {
      var newLocation = new Location(
          command.Location.Address,
          command.Location.City,
          command.Location.State,
          command.Location.ZipCode,
          command.Location.Country,
          command.Location.Latitude,
          command.Location.Longitude);

      car.UpdateLocation(newLocation);
    }

    // Update availability
    car.SetAvailability(command.IsAvailable);

    // Add new images
    if (command.ImagesToAdd != null)
    {
      foreach (var imageUrl in command.ImagesToAdd)
      {
        car.AddImage(imageUrl);
      }
    }

    // Remove images
    if (command.ImagesToRemove != null)
    {
      foreach (var imageUrl in command.ImagesToRemove)
      {
        car.RemoveImage(imageUrl);
      }
    }

    await _carRepository.UpdateAsync(car, cancellationToken);
    await _carRepository.SaveChangesAsync(cancellationToken);
    // await _unitOfWork.SaveChangesAsync(cancellationToken);

    return true;
  }
}