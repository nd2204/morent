using System;

namespace Morent.Application.Features.Rental.Commands;

public record CancelRentalCommand(Guid RentalId, Guid UserId) : ICommand<bool>;
