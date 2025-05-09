namespace Morent.Application.Features.User.Queries;

public record class GetUserProfileImageQuery(Guid UserId) : IQuery<Result<UserProfileImageDto>>;
