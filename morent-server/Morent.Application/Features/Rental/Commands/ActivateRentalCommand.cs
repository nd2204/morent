using System;

namespace Morent.Application.Features.Rental.Commands;

public class ActivateRentalCommand : ICommand<bool>
{
  public Guid RentalId { get; set; }
}