using Morent.Application.Extensions;
using Morent.Core.Exceptions;

namespace Morent.Application.Features.Rental;

public class GetUserRentalsQueryHandler : IQueryHandler<GetUserRentalsQuery, IEnumerable<RentalDto>>
{
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;

  public GetUserRentalsQueryHandler(
      IRentalRepository rentalRepository,
      ICarRepository carRepository)
  {
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
  }

  public async Task<IEnumerable<RentalDto>> Handle(GetUserRentalsQuery query, CancellationToken cancellationToken)
  {
    var rentals = await _rentalRepository.GetRentalsByUserIdAsync(query.UserId, cancellationToken);

    if (query.Status.HasValue)
    {
      rentals = rentals.Where(r => r.Status == query.Status.Value);
    }

    var rentalDtos = new List<RentalDto>();
    foreach (var rental in rentals)
    {
      var car = await _carRepository.GetByIdAsync(rental.CarId, cancellationToken);

      if (car == null) throw new DomainException($"Car with Id={rental.CarId} not found!");

      rentalDtos.Add(new RentalDto
      {
        Id = rental.Id,
        CarInfo = car.ToCarDto(),
        PickupDate = rental.RentalPeriod.Start,
        DropoffDate = rental.RentalPeriod.End,
        PickupLocation = rental.PickupLocation.ToDto(),
        DropoffLocation = rental.DropoffLocation.ToDto(),
        TotalCost = rental.TotalCost.Amount,
        Currency = rental.TotalCost.Currency,
        Status = rental.Status,
        CreatedAt = rental.CreatedAt
      });
    }

    return rentalDtos;
  }
}
