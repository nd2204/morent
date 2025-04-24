using System;
using System.ComponentModel.DataAnnotations;

namespace Morent.Application.DTOs;

  public class CarDto
  {
    public Guid Id { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = null!;
    public FuelType FuelType { get; set; }
    public decimal PricePerDay { get; set; }
    public string Currency { get; set; } = null!;
    public int Capacity { get; set; }
    public List<string> Images { get; set; } = null!;
    public bool IsAvailable { get; set; }
    public LocationDto CurrentLocation { get; set; } = null!;
    public double? AverageRating { get; set; }
    public int ReviewsCount { get; set; }
  }
