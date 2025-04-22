using System;

namespace Morent.Application.DTOs;

  public class CarDto
  {
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public FuelType FuelType { get; set; }
    public decimal PricePerDay { get; set; }
    public string Currency { get; set; }
    public int Capacity { get; set; }
    public List<string> Images { get; set; }
    public bool IsAvailable { get; set; }
    public LocationDto CurrentLocation { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewsCount { get; set; }
  }
