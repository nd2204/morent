using System;

namespace Morent.Application.Features.Car.Commands;

public record class DeleteCarCommand(Guid carId) : ICommand<bool>;
