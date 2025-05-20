namespace Morent.Application.Features.Rental.Queries;

public record class GetRentalsByQuery(PagedQuery query) : IQuery<PagedResult<IEnumerable<RentalDto>>>;
