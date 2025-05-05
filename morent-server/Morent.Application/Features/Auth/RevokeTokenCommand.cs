using System;

namespace Morent.Application.Features.Auth;

public record class RevokeTokenCommand(string refreshToken, Guid userId) : ICommand<Result>;
