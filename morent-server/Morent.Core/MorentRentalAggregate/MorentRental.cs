using Morent.Core.MorentPaymentAggregate;

namespace Morent.Core.MorentRentalAggregate;

public class MorentRental : EntityBase<Guid>, IAggregateRoot
{
  public Guid UserId { get; private set; }
  public Guid CarId { get; private set; }
  public MorentCar Car { get; private set; }
  public DateRange RentalPeriod { get; private set; }
  public Location PickupLocation { get; private set; }
  public Location DropoffLocation { get; private set; }
  public Money TotalCost { get; private set; }
  public MorentRentalStatus Status { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public MorentPayment? Payment { get; private set; }

  private MorentRental() { }

  private MorentRental(Guid rentalId, Guid userId, Guid carId, DateRange rentalPeriod,
               Location pickupLocation, Location dropoffLocation, Money totalCost)
  {
    Guard.Against.Null(rentalId, nameof(rentalId));
    Guard.Against.Null(dropoffLocation, nameof(dropoffLocation));
    Guard.Against.Null(pickupLocation, nameof(pickupLocation));
    Guard.Against.Null(rentalPeriod, nameof(rentalPeriod));
    Guard.Against.Null(totalCost, nameof(totalCost));
    Id = rentalId;
    UserId = userId;
    CarId = carId;
    RentalPeriod = rentalPeriod;
    PickupLocation = pickupLocation;
    DropoffLocation = dropoffLocation;
    TotalCost = totalCost;
    Status = MorentRentalStatus.Reserved;
  }

  public static Result<MorentRental> Create(Guid rentalId, Guid userId, Guid carId, DateRange rentalPeriod,
                                    Location pickupLocation, Location dropoffLocation, Money totalCost)
  {
    if (userId == Guid.Empty)
      return Result.Invalid(new ValidationError("Rental", "User ID cannot be empty."));

    if (carId == Guid.Empty)
      return Result.Invalid(new ValidationError("Rental", "Car ID cannot be empty."));

    if (pickupLocation is null)
      return Result.Invalid(new ValidationError("Rental", "Pickup location cannot be null."));

    if (dropoffLocation is null)
      return Result.Invalid(new ValidationError("Rental", "Dropoff location cannot be empty."));

    var rental = new MorentRental(rentalId, userId, carId, rentalPeriod, pickupLocation, dropoffLocation, totalCost);
    rental.RegisterDomainEvent(new RentalCreatedEvent(rental.Id, carId, userId, rentalPeriod));

    rental.CreatedAt = DateTime.UtcNow;

    return Result.Success(rental);
  }

  public Result ConfirmRental()
  {
    if (Status != MorentRentalStatus.Reserved)
      return Result.Invalid(new ValidationError("Rental", "Rental can only be confirmed when in Reserved status."));

    Status = MorentRentalStatus.Confirmed;
    // UpdateModifiedDate();
    this.RegisterDomainEvent(new RentalConfirmedEvent(Id, CarId, UserId));

    return Result.Success();
  }

  public Result StartRental()
  {
    if (Status != MorentRentalStatus.Confirmed)
      return Result.Error("Rental can only be started when in Confirmed status.");

    Status = MorentRentalStatus.Active;
    // UpdateModifiedDate();
    this.RegisterDomainEvent(new RentalStartedEvent(Id, CarId, UserId));

    return Result.Success();
  }

  public Result CompleteRental()
  {
    if (Status != MorentRentalStatus.Active)
      return Result.Error("Rental can only be completed when in Active status.");

    Status = MorentRentalStatus.Completed;
    // UpdateModifiedDate();
    this.RegisterDomainEvent(new RentalCompletedEvent(Id, CarId, UserId));

    return Result.Success();
  }

  public Result CancelRental()
  {
    if (Status == MorentRentalStatus.Completed || Status == MorentRentalStatus.Cancelled)
      return Result.Error("Cannot cancel a completed or already cancelled rental.");

    Status = MorentRentalStatus.Cancelled;
    // UpdateModifiedDate();
    this.RegisterDomainEvent(new RentalCancelledEvent(Id, CarId, UserId));

    return Result.Success();
  }

  // Only user whom rented this car will be able to review
  public bool CanBeReviewedBy(Guid userId)
  {
    return UserId == userId && Status == MorentRentalStatus.Completed;
  }

  public bool IsActive()
  {
    return Status == MorentRentalStatus.Active;
  }

  public bool CanCancel()
  {
    return Status == MorentRentalStatus.Reserved || Status == MorentRentalStatus.Confirmed;
  }
}
