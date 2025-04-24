using System;
using Morent.Application.Extensions;
using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Car.Queries;

public class GetCarsByQueryHandler : IQueryHandler<GetCarsByQuery, PagedResult<List<CarDto>>>
{
  private readonly ICarRepository _carRepository;

  public GetCarsByQueryHandler(ICarRepository carRepo) {
    _carRepository = carRepo;
  }

  public async Task<PagedResult<List<CarDto>>> Handle(GetCarsByQuery request, CancellationToken cancellationToken)
  {
    var query = request.query;
    var carsQuery = (await _carRepository.GetAvailableCarsAsync(null, null)).AsQueryable();

    // 🔎 Apply Filters
    if (!string.IsNullOrWhiteSpace(query.Brand))
      carsQuery = carsQuery.Where(
        c => c.CarModel.Brand.Contains(query.Brand));

    if (!string.IsNullOrWhiteSpace(query.Type))
      carsQuery = carsQuery.Where(
        c => c.CarModel.ModelName.Contains(query.Type));

    if (query.Capacity.HasValue)
      carsQuery = carsQuery.Where(
        c => c.CarModel.SeatCapacity == query.Capacity.Value);

    if (!string.IsNullOrWhiteSpace(query.FuelType))
      carsQuery = carsQuery.Where(
        c => c.CarModel.FuelType.ToString().ToLower() == query.FuelType.ToLower());

    if (!string.IsNullOrWhiteSpace(query.Gearbox))
      carsQuery = carsQuery.Where(
        c => c.CarModel.Gearbox.ToString().ToLower() == query.Gearbox.ToLower());

    if (!string.IsNullOrWhiteSpace(query.Location))
      carsQuery = carsQuery.Where(
        c => c.CurrentLocation.City.Contains(query.Location));

    if (query.MinPrice.HasValue)
      carsQuery = carsQuery.Where(
        c => c.PricePerDay.Amount >= query.MinPrice.Value);

    if (query.MaxPrice.HasValue)
      carsQuery = carsQuery.Where(
        c => c.PricePerDay.Amount <= query.MaxPrice.Value);

    if (!string.IsNullOrWhiteSpace(query.Search))
      carsQuery = carsQuery.Where(c =>
          c.CarModel.Brand.Contains(query.Search) ||
          c.CarModel.ModelName.Contains(query.Search));

    // 📦 Get total count before pagination
    var totalRecords = carsQuery.Count();

    // 📋 Apply Sorting (basic example)
    if (!string.IsNullOrWhiteSpace(query.Sort))
    {
      if (query.Sort.Equals("price_asc", StringComparison.OrdinalIgnoreCase))
      {
        carsQuery = carsQuery.OrderBy(c => c.PricePerDay.Amount);
      }
      else if (query.Sort.Equals("price_desc", StringComparison.OrdinalIgnoreCase))
      {
        carsQuery = carsQuery.OrderByDescending(c => c.PricePerDay.Amount);
      }
      else if (query.Sort.Equals("year_desc", StringComparison.OrdinalIgnoreCase))
      {
        carsQuery = carsQuery.OrderByDescending(c => c.CarModel.Year);
      }
      else
      {
        carsQuery = carsQuery.OrderBy(c => c.CarModel.Brand);
      }
    }
    else
    {
      carsQuery = carsQuery.OrderBy(c => c.CarModel.Brand);
    }

    // 📑 Apply Pagination
    var skip = (query.Page - 1) * query.PageSize;
    var cars = carsQuery
        .Skip(skip)
        .Take(query.PageSize)
        .Select(c => c.ToCarDto())
        .ToList();

    return new PagedResult<List<CarDto>>(
      new PagedInfo(query.Page, query.PageSize, totalRecords / query.PageSize, totalRecords),
      cars
    );
  }
}
