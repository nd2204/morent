using Morent.Application.Extensions;

namespace Morent.Application.Features.Car.Queries;

public class GetCarsByQueryHandler : IQueryHandler<GetCarsByQuery, PagedResult<IEnumerable<CarDto>>>
{
  private readonly ICarRepository _carRepository;

  public GetCarsByQueryHandler(ICarRepository carRepo) {
    _carRepository = carRepo;
  }

  public async Task<PagedResult<IEnumerable<CarDto>>> Handle(GetCarsByQuery request, CancellationToken cancellationToken)
  {
    var query = request.query;
    CarFilter? carFilter = query.carFilter;
    var carsQuery = (await _carRepository.GetAvailableCarsAsync(null, null)).AsQueryable();

    if (carFilter != null)
    {
      carsQuery = ApplyCarFilter(carsQuery, carFilter);
    }

    var pagedQuery = request.query.pagedQuery;

    // Get total count before pagination
    int totalRecords = carsQuery.Count();
    int page = pagedQuery.Page;
    int pageSize = pagedQuery.PageSize;

    // Apply Pagination
    int skip = (page - 1) * pageSize;
    var cars = carsQuery
        .Skip(skip)
        .Take(pageSize)
        .Select(c => c.ToCarDto())
        .ToList();

    return new PagedResult<IEnumerable<CarDto>>(
      new PagedInfo(page, pageSize, (totalRecords + pageSize - 1) / pageSize, totalRecords),
      cars
    );
  }

  private IQueryable<MorentCar> ApplyCarFilter(IQueryable<MorentCar> cars, CarFilter filter)
  {
    // Apply Filters
    if (!string.IsNullOrWhiteSpace(filter.Brand))
      cars = cars.Where(
        c => c.CarModel.Brand.Contains(filter.Brand));

    if (!string.IsNullOrWhiteSpace(filter.Type))
      cars = cars.Where(
        c => c.CarModel.ModelName.Contains(filter.Type));

    if (filter.Capacity.HasValue)
      cars = cars.Where(
        c => c.CarModel.SeatCapacity == filter.Capacity.Value);

    if (!string.IsNullOrWhiteSpace(filter.FuelType))
      cars = cars.Where(
        c => c.CarModel.FuelType.ToString().ToLower() == filter.FuelType.ToLower());

    if (!string.IsNullOrWhiteSpace(filter.Gearbox))
      cars = cars.Where(
        c => c.CarModel.Gearbox.ToString().ToLower() == filter.Gearbox.ToLower());

    if (!string.IsNullOrWhiteSpace(filter.Location))
      cars = cars.Where(
        c => c.CurrentLocation.City.Contains(filter.Location));

    if (filter.MinPrice.HasValue)
      cars = cars.Where(
        c => c.PricePerDay.Amount >= filter.MinPrice.Value);

    if (filter.MaxPrice.HasValue)
      cars = cars.Where(
        c => c.PricePerDay.Amount <= filter.MaxPrice.Value);

    // Search for matching car models (by name or make)
    if (!string.IsNullOrWhiteSpace(filter.Search))
      cars = cars.Where(c =>
          c.CarModel.Brand.Contains(filter.Search) ||
          c.CarModel.ModelName.Contains(filter.Search));

    // Apply Sorting
    if (!string.IsNullOrWhiteSpace(filter.Sort))
    {
      if (filter.Sort.Equals("price_asc", StringComparison.OrdinalIgnoreCase))
      {
        cars = cars.OrderBy(c => c.PricePerDay.Amount);
      }
      else if (filter.Sort.Equals("price_desc", StringComparison.OrdinalIgnoreCase))
      {
        cars = cars.OrderByDescending(c => c.PricePerDay.Amount);
      }
      else if (filter.Sort.Equals("year_desc", StringComparison.OrdinalIgnoreCase))
      {
        cars = cars.OrderByDescending(c => c.CarModel.Year);
      }
      else
      {
        cars = cars.OrderBy(c => c.CarModel.Brand);
      }
    }
    else
    {
      cars = cars.OrderBy(c => c.CarModel.Brand);
    }
    return cars;
  }
}
