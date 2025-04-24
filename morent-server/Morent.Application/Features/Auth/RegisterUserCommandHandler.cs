namespace Morent.Application.Features.Auth;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResponse>
{
  private readonly IUserRepository _userRepository;
  private readonly IAuthService _authService;
  // private readonly IUnitOfWork _unitOfWork;

  public RegisterUserCommandHandler(
      IUserRepository userRepository,
      // IUnitOfWork unitOfWork,
      IAuthService authService)
  {
    _userRepository = userRepository;
    _authService = authService;
    // _unitOfWork = unitOfWork;
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
