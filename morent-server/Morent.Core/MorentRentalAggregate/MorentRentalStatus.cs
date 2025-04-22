using System;

namespace Morent.Core.MorentRentalAggregate;

public enum MorentRentalStatus
{
  Reserved,
  Confirmed,
  Active,
  Completed,
  Cancelled
}