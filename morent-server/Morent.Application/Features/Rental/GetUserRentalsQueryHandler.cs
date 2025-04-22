namespace Morent.Application.Features.Rental;

public class GetUserRentalsQueryHandler : IQueryHandler<GetUserRentalsQuery, IEnumerable<RentalDto>>
{
  private readonly IRentalRepository _rentalRepository;
  private readonly ICarRepository _carRepository;
  private readonly ICurrentUserService _currentUserService;

  public GetUserRentalsQueryHandler(
      IRentalRepository rentalRepository,
      ICarRepository carRepository,
      ICurrentUserService currentUserService)
  {
    _rentalRepository = rentalRepository;
    _carRepository = carRepository;
    _currentUserService = currentUserService;
  }

  public async Task<IEnumerable<RentalDto>> Handle(GetUserRentalsQuery query, CancellationToken cancellationToken)
  {
    if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
      throw new UnauthorizedAccessException("User must be authenticated to view rentals");

    var currentUserId = _currentUserService.UserId.Value;

    // Only allow users to see their own rentals, unless they're an admin
    if (query.UserId != currentUserId && _currentUserService.Role != MorentUserRole.Admin)
      throw new UnauthorizedAccessException("You can only view your own rentals");

    var rentals = await _rentalRepository.GetRentalsByUserIdAsync(query.UserId, cancellationToken);

    if (query.Status.HasValue)
    {
      rentals = rentals.Where(r => r.Status == query.Status.Value);
    }

    var rentalDtos = new List<RentalDto>();
    foreach (var rental in rentals)
    {
      var car = await _carRepository.GetByIdAsync(rental.CarId, cancellationToken);

      rentalDtos.Add(new RentalDto
      {
        Id = rental.Id,
        CarId = rental.CarId,
        CarBrand = car?.Brand ?? "Unknown",
        CarModel = car?.Model ?? "Unknown",
        PickupDate = rental.RentalPeriod.Start,
        DropoffDate = rental.RentalPeriod.End,
        PickupLocation = new LocationDto
        {
          Address = rental.PickupLocation.Address,
          City = rental.PickupLocation.City,
          State = rental.PickupLocation.State,
          ZipCode = rental.PickupLocation.ZipCode,
          Country = rental.PickupLocation.Country,
          Latitude = rental.PickupLocation.Latitude,
          Longitude = rental.PickupLocation.Longitude
        },
        DropoffLocation = new LocationDto
        {
          Address = rental.DropoffLocation.Address,
          City = rental.DropoffLocation.City,
          State = rental.DropoffLocation.State,
          ZipCode = rental.DropoffLocation.ZipCode,
          Country = rental.DropoffLocation.Country,
          Latitude = rental.DropoffLocation.Latitude,
          Longitude = rental.DropoffLocation.Longitude
        },
        TotalCost = rental.TotalCost.Amount,
        Currency = rental.TotalCost.Currency,
        Status = rental.Status,
        CreatedAt = rental.CreatedAt
      });
    }

    return rentalDtos;
  }
}
