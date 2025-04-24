using System;

namespace Morent.Application.Features.Car.DTOs;

public class GetCarsQuery
{
  public string? Brand { get; set; }
  public string? Type { get; set; }
  public int? Capacity { get; set; }
  public string? FuelType { get; set; }
  public string? Gearbox { get; set; }
  public decimal? MinPrice { get; set; }
  public decimal? MaxPrice { get; set; }
  public int? Rating { get; set; }
  public string? Location { get; set; }
  public string? Search { get; set; }
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 10;
  public string? Sort { get; set; }
}
