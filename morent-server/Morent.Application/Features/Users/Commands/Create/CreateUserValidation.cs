using FluentValidation;
using Morent.Core.MorentUserAggregate;
using Morent.Core.MorentUserAggregate.Specifications;

namespace Morent.Application.Features.Users.Commands.Create;

public class CreateUserValidation : AbstractValidator<CreateUserCommand>
{
  private readonly IRepository<MorentUser> repository_;

  public CreateUserValidation(IRepository<MorentUser> repository)
  {
    this.repository_ = repository;

    RuleFor(u => u.email)
      .NotEmpty().WithMessage("{PropertyName} is required")
      .NotNull()
      .EmailAddress()
      ;

    RuleFor(u => u.username)
      .NotEmpty().WithMessage("{PropertyName} is required")
      .NotNull()
      .MustAsync(UsernameUnique)
      ;

    RuleFor(u => u.password).NotEmpty().WithMessage("Your password cannot be empty")
        .MinimumLength(8).WithMessage("Your password length must be at least 8.")
        .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
        .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
        .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
        .Matches(@"[\!\@\#\$\%\^&\*\(\)\-_\+\=\[\]\|\\\;\:\,\<\.\>\/\?\`\~]+")
          .WithMessage("Your password must contain at least one special character (e.g: !? *.).");
  }

  private async Task<bool> UsernameUnique(string username, CancellationToken token)
  {
    return await repository_.AnyAsync(new UserByUsernameOrEmailSpec(username));
  }
}
