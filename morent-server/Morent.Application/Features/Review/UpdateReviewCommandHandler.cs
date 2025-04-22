using System;

namespace Morent.Application.Features.Review;

public class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, bool>
{
  private readonly IReviewRepository _reviewRepository;

  public UpdateReviewCommandHandler(IReviewRepository reviews)
  {
    _reviewRepository = reviews;
  }

  public Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
