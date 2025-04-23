using System;

namespace Morent.Application.Features.Rental;

public record CancelRentalCommand(Guid RentalId, Guid UserId) : ICommand<bool>;
