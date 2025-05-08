using System;

namespace Morent.Application.Features.Car.Queries;

public record class GetCarImagesQuery(Guid CarId) : IQuery<Result<IEnumerable<CarImageDto>>>;
