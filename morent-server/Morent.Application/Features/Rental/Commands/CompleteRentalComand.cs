using System;

namespace Morent.Application.Features.Rental.Commands;

public record CompleteRentalCommand(Guid RentalId) : ICommand<bool>;