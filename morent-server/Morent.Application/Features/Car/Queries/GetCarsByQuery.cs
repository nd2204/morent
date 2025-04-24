using System;
using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Car.Queries;

public record class GetCarsByQuery(GetCarsQuery query) : IQuery<PagedResult<List<CarDto>>>;
