using Morent.Application.Exceptions;
using Morent.Application.Interfaces;
using Morent.Core.MorentUserAggregate;

namespace Morent.Application.Features.Users.Commands.Create;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Result<UserDto>>
{
  private readonly IRepository<MorentUser> repository_;
  private readonly IUserService userService_;
  private readonly IAuthService authService_;

  public CreateUserHandler(
    IRepository<MorentUser> repository,
    IAuthService authService,
    IUserService userService)
  {
    repository_ = repository;
    userService_ = userService;
    authService_ = authService;
  }

  public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {
    var validator = new CreateUserValidation(repository_);
    var validationResult = await validator.ValidateAsync(request);

    if (!validationResult.IsValid) {
      return Result.Invalid();
    }

    var result = await authService_.SignupAsync(request.username, request.password, request.email);

    if (!result.IsSuccess) {
      return Result.Error("");
    }

    var value = result.Value;
    return new UserDto { Id = value.Id.ToString(), Name = value.Name };
  }
}
 
