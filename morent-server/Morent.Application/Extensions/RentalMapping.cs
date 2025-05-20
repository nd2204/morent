using System;

namespace Morent.Application.Extensions;

public static class RentalMapping
{
  public static RentalDto ToDto(this MorentRental rental)
  {
    return new RentalDto
    {
      Id = rental.Id,
      UserId = rental.UserId,
      CarId = rental.CarId,
      PickupDate = rental.RentalPeriod.Start,
      DropoffDate = rental.RentalPeriod.End,
      PickupLocation = rental.PickupLocation.ToDto(),
      DropoffLocation = rental.DropoffLocation.ToDto(),
      TotalCost = rental.TotalCost.Amount,
      Status = rental.Status.ToString(),
      Currency = rental.TotalCost.Currency,
      CreatedAt = rental.CreatedAt
    };
  }
}
