using System;

namespace Morent.Application.Features.Users.Commands.Login;

public record class LoginUserCommand(string usernameOrEmail, string password) : ICommand<Result<string>> ;
