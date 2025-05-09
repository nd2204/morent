using System;

namespace Morent.Application.Features.Car.Commands;

public class UpdateCarCommandHandler : ICommandHandler<UpdateCarCommand, Result>
{
  private readonly ICarRepository _carRepository;
  private readonly ICarImageService _carImageService;
  private readonly IUnitOfWork _unitOfWork;

  public UpdateCarCommandHandler(
      IUnitOfWork unitOfWork,
      ICarImageService carImageService,
      ICarRepository carRepository)
  {
    _unitOfWork = unitOfWork;
    _carRepository = carRepository;
    _carImageService = carImageService;
  }

  public async Task<Result> Handle(UpdateCarCommand command, CancellationToken cancellationToken)
  {
    var car = await _carRepository.GetByIdAsync(command.Id, cancellationToken);
    if (car == null)
      return Result.NotFound($"Car with ID {command.Id} not found");

    // Update price if provided
    if (command.PricePerDay > 0)
    {
      var newPriceResult = Money.Create(command.PricePerDay, command.Currency);
      if (newPriceResult.IsInvalid())
        return Result.Invalid(newPriceResult.ValidationErrors);
      car.UpdatePrice(newPriceResult.Value);
    }

    // Update location if provided
    if (command.Location != null)
    {
      var newLocationResult = Location.Create(
          command.Location.Address,
          command.Location.City,
          command.Location.Country
          );
        
      if (newLocationResult.IsInvalid())
        return Result.Invalid(newLocationResult.ValidationErrors);

      car.UpdateLocation(newLocationResult.Value);
    }

    // Update availability
    car.SetAvailability(command.IsAvailable);

    // Add new images
    if (command.ImagesToAdd != null)
    {
      foreach (var request in command.ImagesToAdd)
      {
        await _carImageService.AddCarImageAsync(car.Id, request);
      }
    }

    // Remove images
    if (command.ImagesToDelete != null)
    {
      foreach (var imageUrl in command.ImagesToDelete)
      {
        car.RemoveImage(imageUrl);
      }
    }

    await _carRepository.UpdateAsync(car, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}