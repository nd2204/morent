using System;

namespace Morent.Application.Features.User.Queries;

public record class GetUserByIdQuery(Guid UserId) : IQuery<Result<UserDto>>;
