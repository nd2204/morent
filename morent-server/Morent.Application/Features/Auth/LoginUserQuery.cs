using System;

namespace Morent.Application.Features.Auth;

public record class LoginUserQuery(string LoginId, string Password) : IQuery<AuthResponse>;
