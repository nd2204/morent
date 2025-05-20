using System;

namespace Morent.Application.Features.Rental.Queries;

public class GetUserRentalByIdQuery : IQuery<RentalDto>
{
  public Guid UserId { get; set; }
  public Guid RentalId { get; set; }
}
