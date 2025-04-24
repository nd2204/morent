using System;

namespace Morent.Application.Features.Rental;

public record CompleteRentalCommand(Guid RentalId) : ICommand<bool>;