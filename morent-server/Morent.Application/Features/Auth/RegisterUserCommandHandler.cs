namespace Morent.Application.Features.Auth;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResponse>
{
  private readonly IUserRepository _userRepository;
  private readonly IAuthService _authService;

  public RegisterUserCommandHandler(
      IUserRepository userRepository,
      IAuthService authService)
  {
    _userRepository = userRepository;
    _authService = authService;
  }

  public async Task<AuthResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
  {
    var request = new RegisterRequest {
      Name = command.Name,
      Email = command.Email,
      Username = command.Username,
      Password = command.Password,
    };

    return await _authService.RegisterAsync(request);
  }
}
