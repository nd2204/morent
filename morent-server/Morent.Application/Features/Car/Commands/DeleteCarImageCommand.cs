using System;

namespace Morent.Application.Features.Car.Commands;

public record class DeleteCarImageCommand(Guid CarId, Guid ImageId) : ICommand<Result>;
