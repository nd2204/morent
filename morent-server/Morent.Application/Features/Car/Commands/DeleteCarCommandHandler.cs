using System;

namespace Morent.Application.Features.Car.Commands;

public class DeleteCarCommandHandler(ICarRepository _carRepository) : ICommandHandler<DeleteCarCommand, bool>
{
  public async Task<bool> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
  {
    var car = await _carRepository.GetByIdAsync(request.carId, cancellationToken);
    if (car == null)
      return false;

    await _carRepository.DeleteAsync(car, cancellationToken);
    await _carRepository.SaveChangesAsync();
    return true;
  }
}
