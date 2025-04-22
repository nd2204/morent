using System;

namespace Morent.Application.Features.Rental;

public record CancelRentalCommand(Guid RentalId) : ICommand<bool>;
