using System;

namespace Morent.Application.DTOs;

public class LocationDto
{
  public string Address { get; set; } = null!;
  public string City { get; set; } = null!;
  public string State { get; set; } = null!;
  public string ZipCode { get; set; } = null!;
  public string Country { get; set; } = null!;
  public double? Latitude { get; set; }
  public double? Longitude { get; set; }
}
