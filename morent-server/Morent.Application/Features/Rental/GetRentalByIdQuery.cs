using System;

namespace Morent.Application.Features.Rental;

public class GetRentalByIdQuery : IQuery<RentalDto>
{
  public Guid Id { get; set; }
}
