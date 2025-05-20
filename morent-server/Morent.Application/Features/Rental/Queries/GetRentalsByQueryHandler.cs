namespace Morent.Application.Features.Rental.Queries;

public class GetRentalsByQueryHandler(
  IRentalRepository rentalRepository) : IQueryHandler<GetRentalsByQuery, PagedResult<IEnumerable<RentalDto>>>
{
  private readonly IRentalRepository _rentalRepository = rentalRepository;

  public async Task<PagedResult<IEnumerable<RentalDto>>> Handle(GetRentalsByQuery request, CancellationToken cancellationToken)
  {
    var rentalQuery = (await _rentalRepository.ListAsync(cancellationToken)).AsQueryable();

    // Get total count before pagination
    int totalRecords = rentalQuery.Count();
    int page = request.query.Page;
    int pageSize = request.query.PageSize;

    // Apply Pagination
    int skip = (page - 1) * pageSize;
    var rentalDtos = rentalQuery
        .Skip(skip)
        .Take(pageSize)
        .Select(c => c.ToDto())
        .ToList();

    return new PagedResult<IEnumerable<RentalDto>>(
      new PagedInfo(page, pageSize, (totalRecords + pageSize - 1) / pageSize, totalRecords),
      rentalDtos
    );
  }
}
