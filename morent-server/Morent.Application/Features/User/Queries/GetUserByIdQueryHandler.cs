using System;

namespace Morent.Application.Features.User.Queries;

public class GetUserByIdQueryHandler(
  IUserRepository repository) : IQueryHandler<GetUserByIdQuery, Result<UserDto>>
{
  private readonly IUserRepository _userRepository = repository;
  public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByIdAsync(request.UserId);
    if (user == null)
      return Result.NotFound($"User with ID {request.UserId} not found.");

    return Result.Success();
  }
}
