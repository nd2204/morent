using System;

namespace Morent.Application.Features.Car.Queries;

public record class GetNearCarsLocationQuery(double Longitude, double Latitude, double MaxDistanceKm) : IQuery<Result<IEnumerable<CarLocationDto>>>;
