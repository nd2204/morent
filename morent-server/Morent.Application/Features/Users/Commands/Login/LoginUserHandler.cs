using System;
using Morent.Core.Interfaces;

namespace Morent.Application.Features.Users.Commands.Login;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<UserDto>>
{
  private readonly IUserService service_;

  public LoginUserHandler(IUserService service_)
  {
    this.service_ = service_;
  }

  public async Task<Result<UserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
  {
    var result = await service_.GetUserByCredentialAsync(request.usernameOrEmail, request.password);

    if (!result.IsSuccess) {
      return Result.NotFound("There no valid credential associated with the provided username or email");
    }

    var value = result.Value;

    return Result.Success(new UserDto {
      Id = value.Id.ToString(),
      Email = value.Email,
      Name = value.Name,
      Username = value.Username,
    });
  }
}
