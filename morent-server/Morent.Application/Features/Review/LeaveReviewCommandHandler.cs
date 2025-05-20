using System;

namespace Morent.Application.Features.Review;

public class LeaveReviewCommandHandler : ICommandHandler<LeaveReviewCommand, Result<ReviewDto>>
{
  private readonly IUserService _userService;

  public LeaveReviewCommandHandler(IUserService userService)
  {
    _userService = userService;
  }

  public async Task<Result<ReviewDto>> Handle(LeaveReviewCommand command, CancellationToken cancellationToken)
  {
    return await _userService.LeaveReviewAsync(
      command.UserId,
      command.Request.RentalId,
      command.Request.Rating,
      command.Request.Comment
      );
  }
}