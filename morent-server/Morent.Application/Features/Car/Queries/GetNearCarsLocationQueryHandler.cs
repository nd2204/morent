using System;

namespace Morent.Application.Features.Car.Queries;

public class GetNearCarsLocationQueryHandler(
  ICarRepository carRepo,
  IImageService imageService
) : IQueryHandler<GetNearCarsLocationQuery, Result<IEnumerable<CarLocationDto>>>
{
  // Radius of the Earth in kilometers
  private const double EarthRadiusKm = 6371.0;

  private readonly ICarRepository _carRepo = carRepo;
  private readonly IImageService _imageService = imageService;

  public async Task<Result<IEnumerable<CarLocationDto>>> Handle(GetNearCarsLocationQuery request, CancellationToken cancellationToken)
  {
    var cars = await _carRepo.GetAvailableCarsAsync(null, null);

    List<CarLocationDto> carLocationDtos = new();
    foreach (var car in cars) {
      var dto = car.ToCarLocationDto();
      var result = await _imageService.GetImageByIdAsync(car.Images.First().ImageId);
      if (!result.IsSuccess)
      {
        var placeholderImageResult = await _imageService.GetPlaceHolderImageAsync();
        dto.ImageUrl = placeholderImageResult.Value.Url;
        continue;
      }
      dto.ImageUrl = result.Value.Url;
      carLocationDtos.Add(dto);
    }

    return Result.Success(carLocationDtos
      .AsQueryable()
      .Where(c => IsLocationClose(c.Latitude, c.Longitude, request.Latitude, request.Longitude, request.MaxDistanceKm))
      .AsEnumerable()
    );
  }

  // Haversine formula to calculate distance between two lat/lng points
  public static bool IsLocationClose(
      double lat1, double lon1,
      double lat2, double lon2,
      double maxDistanceKm)
  {
    double dLat = DegreesToRadians(lat2 - lat1);
    double dLon = DegreesToRadians(lon2 - lon1);

    double a = Math.Pow(Math.Sin(dLat / 2), 2) +
               Math.Cos(DegreesToRadians(lat1)) *
               Math.Cos(DegreesToRadians(lat2)) *
               Math.Pow(Math.Sin(dLon / 2), 2);

    double c = 2 * Math.Asin(Math.Sqrt(a));
    double distance = EarthRadiusKm * c;
    Console.WriteLine($"distance from {lat1}-{lon1} to {lat2}-{lon2} is {distance}km");

    return distance <= maxDistanceKm;
  }

  private static double DegreesToRadians(double degrees)
  {
    return degrees * Math.PI / 180.0;
  }
}
