using System;

namespace Morent.Application.Features.Rental;

public record class GetUserRentalsQuery(Guid UserId, MorentRentalStatus? Status) : IQuery<Result<IEnumerable<RentalDto>>>;