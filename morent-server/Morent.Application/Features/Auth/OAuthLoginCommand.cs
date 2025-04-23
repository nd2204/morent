using System;

namespace Morent.Application.Features.Auth;

public record class OAuthLoginCommand(string Provider, string IdToken) : ICommand<AuthResponse>;
