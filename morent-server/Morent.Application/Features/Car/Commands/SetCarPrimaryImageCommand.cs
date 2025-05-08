using System;

namespace Morent.Application.Features.Car.Commands;

public record class SetCarPrimaryImageCommand(Guid CarId, Guid ImageId) : ICommand<Result<CarImageDto>>;
