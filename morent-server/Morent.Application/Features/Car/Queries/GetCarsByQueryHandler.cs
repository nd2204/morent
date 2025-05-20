using Morent.Application.Extensions;

namespace Morent.Application.Features.Car.Queries;

public class GetCarsByQueryHandler : IQueryHandler<GetCarsByQuery, PagedResult<IEnumerable<CarDto>>>
{
  private readonly ICarRepository _carRepository;
  private readonly IImageService _imageService;

  public GetCarsByQueryHandler(ICarRepository carRepo, IImageService imageService)
  {
    _carRepository = carRepo;
    _imageService = imageService;
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

    cars.ForEach(car => car.Images.ForEach(
      async image => image = await SetCarImageUrl(image)));

    return new PagedResult<IEnumerable<CarDto>>(
      new PagedInfo(page, pageSize, (totalRecords + pageSize - 1) / pageSize, totalRecords),
      cars
    );
  }

  private async Task<CarImageDto> SetCarImageUrl(CarImageDto carImage)
  {
    var result = await _imageService.GetImageByIdAsync(carImage.ImageId);
    if (!result.IsSuccess)
    {
      var placeholderImageResult = await _imageService.GetPlaceHolderImageAsync();
      carImage.Url = placeholderImageResult.Value.Url;
      return carImage;
    }
    carImage.Url = result.Value.Url;
    return carImage;
  }

  private IQueryable<MorentCar> ApplyCarFilter(IQueryable<MorentCar> cars, CarFilter filter)
  {
    // Apply Filters
    if (!string.IsNullOrWhiteSpace(filter.Brand))
      cars = cars.Where(
        c => c.CarModel.Brand.ToLower().Contains(filter.Brand.ToLower()));

    if (!string.IsNullOrWhiteSpace(filter.Type))
      cars = cars.Where(
        c => c.CarModel.CarType.ToString().ToLower() == filter.Type.ToLower());

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
    {
      cars = cars.Where(
        c => !string.IsNullOrWhiteSpace(c.CurrentLocation.City)
          ? c.CurrentLocation.City.Contains(filter.Location)
          : true);
  }

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
      switch(filter.Sort)
      {
        case "price_asc": cars = cars.OrderBy(c => c.PricePerDay.Amount); break;
        case "price_desc": cars = cars.OrderByDescending(c => c.PricePerDay.Amount); break;
        case "year_asc": cars = cars.OrderBy(c => c.CarModel.Year); break;
        case "year_desc": cars = cars.OrderByDescending(c => c.CarModel.Year); break;
        case "reviews_asc": cars = cars.OrderBy(c => c.Reviews.Count); break;
        case "reviews_desc": cars = cars.OrderByDescending(c => c.Reviews.Count); break;
        default: cars = cars.OrderBy(c => c.CarModel.Brand); break;
      }
    }
    else
{
  cars = cars.OrderBy(c => c.CarModel.Brand);
}
return cars;
  }
}
