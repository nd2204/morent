using System;

namespace Morent.Application.Features.Rental.Queries;

public record class GetUserRentalsQuery(Guid UserId, MorentRentalStatus? Status) : IQuery<Result<IEnumerable<RentalDto>>>;