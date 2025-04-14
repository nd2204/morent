namespace Morent.Application.Features.Users.Commands.Create;

public record class CreateUserCommand(
  string username, string password, string email
  ) : ICommand<Result<CreatedUserDto>>;
