using System;

namespace Morent.Application.Features.Rental;

public class GetUserRentalsQuery : IQuery<IEnumerable<RentalDto>>
{
  public Guid UserId { get; set; }
  public MorentRentalStatus? Status { get; set; }
}
