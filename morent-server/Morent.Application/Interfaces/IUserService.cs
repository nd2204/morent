using System;

namespace Morent.Application.Interfaces;

public interface IUserService
{
  Task<Result<ReviewDto>> LeaveReviewAsync(Guid userId, Guid rentalId, int rating, string comment, CancellationToken cancellationToken = default);
  Task<Result<RentalDto>> CreateRentalAsync(Guid userId, Guid carId, CreateRentalRequest request, CancellationToken cancellationToken = default);
  Task<Result> ActivateRentalAsync(Guid userId, Guid rentalId, CancellationToken cancellationToken = default);
  Task<Result> CancelRentalAsync(Guid userId, Guid rentalId, CancellationToken cancellationToken = default);
  Task<Result> CompleteRentalAsync(Guid userId, Guid rentalId, CancellationToken cancellationToken = default);
}
