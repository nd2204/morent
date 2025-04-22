using System;

namespace Morent.Application.Features.Rental;

public class ActivateRentalCommand : ICommand<bool>
{
  public Guid RentalId { get; set; }
}