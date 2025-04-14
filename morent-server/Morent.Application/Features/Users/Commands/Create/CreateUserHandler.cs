using Morent.Application.Exceptions;
using Morent.Core.Interfaces;
using Morent.Core.MorentUserAggregate;

namespace Morent.Application.Features.Users.Commands.Create;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Result<CreatedUserDto>>
{
  private readonly IRepository<MorentUser> repository_;
  private readonly IUserService service_;

  public CreateUserHandler(IRepository<MorentUser> repository, IUserService service)
  {
    repository_ = repository;
    service_ = service;
  }

  public async Task<Result<CreatedUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {
    var validator = new CreateUserValidation(repository_);
    var validationResult = await validator.ValidateAsync(request);

    if (!validationResult.IsValid) {
      return Result.Invalid();
    }

    var result = await service_.CreateUserAsync(request.username, request.password, request.email);
    if (!result.IsSuccess) {
      return Result.Error();
    }

    var value = result.Value;
    return new CreatedUserDto { Id = value.Id.ToString(), Name = value.Name, Role = value.Role };
  }
}
 
