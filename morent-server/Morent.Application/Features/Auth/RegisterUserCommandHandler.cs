namespace Morent.Application.Features.Auth;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResponse>
{
  private readonly IUserRepository _userRepository;
  private readonly IAuthService _authService;
  private readonly IPasswordHasher _passwordHasher;
  // private readonly IUnitOfWork _unitOfWork;

  public RegisterUserCommandHandler(
      IUserRepository userRepository,
      IAuthService authService,
      IPasswordHasher passwordHasher)
      // IUnitOfWork unitOfWork)
  {
    _userRepository = userRepository;
    _authService = authService;
    _passwordHasher = passwordHasher;
    // _unitOfWork = unitOfWork;
  }

  public async Task<AuthResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
  {
    // Validate email is unique
    if (await _userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
      throw new ApplicationException("Email is already in use");

    // Create the user with hashed password
    var userId = Guid.NewGuid();
    var passwordHash = _passwordHasher.HashPassword(command.Password);
    var email = new Email(command.Email);

    var user = new MorentUser(command.Name, command.Username, email, passwordHash);
    await _userRepository.AddAsync(user, cancellationToken);
    await _userRepository.SaveChangesAsync();
    // await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Return authentication response
    var authRequest = new LoginRequest
    {
      LoginId = command.Email,
      Password = command.Password
    };

    return await _authService.AuthenticateAsync(authRequest, cancellationToken);
  }
}
