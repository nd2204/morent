using Morent.Application.Features.Car.DTOs;

namespace Morent.Application.Features.Car;

public record class GetCarByIdQuery(Guid Id) : IQuery<CarDetailDto>;
