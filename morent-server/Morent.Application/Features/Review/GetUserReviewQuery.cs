namespace Morent.Application.Features.Review;

public record class GetUserReviewsQuery(Guid UserId) : IQuery<Result<IEnumerable<UserCarsReviewDto>>>;
