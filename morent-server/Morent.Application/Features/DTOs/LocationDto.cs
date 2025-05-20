namespace Morent.Application.Features.DTOs;

public class LocationDto
{
  public string? City { get; set; }
  public string? Address { get; set; }
  public string? Country { get; set; }
  public required double Longitude { get; set; }
  public required double Latitude { get; set; }
}