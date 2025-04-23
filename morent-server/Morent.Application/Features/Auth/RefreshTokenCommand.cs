using System;

namespace Morent.Application.Features.Auth;

public record class RefreshTokenCommand(string RefreshToken) : ICommand<AuthResponse>;