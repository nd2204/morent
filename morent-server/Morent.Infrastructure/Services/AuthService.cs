using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Morent.Application.Interfaces;
using Morent.Core.ValueObjects;
using Morent.Infrastructure.Settings;
using Morent.Application.Repositories;
using Morent.Application.Features.Auth.DTOs;
using Ardalis.Result;

namespace Morent.Infrastructure.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IOAuthService _oAuthService;
  private readonly AppSettings _appSettings;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IHttpClientFactory _httpClientFactory;

  public AuthService(
    IUserRepository userRepository,
    IOptions<AppSettings> appSettings,
    IOAuthService oAuthService,
    IUnitOfWork unitOfWork,
    IHttpClientFactory httpClientFactory)
  {
    _userRepository = userRepository;
    _appSettings = appSettings.Value;
    _unitOfWork = unitOfWork;
    _httpClientFactory = httpClientFactory;
    _oAuthService = oAuthService;
  }

  public RefreshToken GenerateRefreshToken()
  {
    // Generate a secure random token
    var randomBytes = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomBytes);

    var token = Convert.ToBase64String(randomBytes);
    var expiresAt = DateTime.Now.AddDays(_appSettings.RefreshTokenExpiryDays);

    return new RefreshToken(token, expiresAt);
  }

  public string GenerateJwtToken(MorentUser user)
  {
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

    var securityKey = new SymmetricSecurityKey(
     Encoding.UTF8.GetBytes(_appSettings.JwtSecret)
     );
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _appSettings.JwtIssuer,
        audience: _appSettings.JwtAudience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(_appSettings.JwtExpiryMinutes),
        signingCredentials: credentials
        );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }


  public string HashPassword(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password, 12);
  }

  public bool VerifyPassword(string password, string passwordHash)
  {
    return BCrypt.Net.BCrypt.Verify(password, passwordHash);
  }

  public AuthResponse GenerateAuthResponse(MorentUser user)
  {
    var jwtToken = GenerateJwtToken(user);
    var refreshToken = GenerateRefreshToken();

    // Create response with both tokens
    return new AuthResponse
    {
      AccessToken = jwtToken,
      RefreshToken = refreshToken.Token,
      ExpiresAt = DateTime.UtcNow.AddMinutes(_appSettings.JwtExpiryMinutes),
      User = new UserDto
      {
        UserId = user.Id,
        Name = user.Name,
        Email = user.Email.Value,
      }
    };
  }

  public async Task<Result<AuthResponse>> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default)
  {
    // Find user by email
    var user = await _userRepository.GetByUsernameOrEmail(request.LoginId);
    if (user == null)
      return Result.Invalid(new ValidationError("Invalid username/email or password"));

    // Validate password
    if (user.PasswordHash != null)
    {
      bool isPasswordValid = VerifyPassword(request.Password, user.PasswordHash);
      if (!isPasswordValid)
        return Result.Invalid(new ValidationError("Invalid username/email or password"));
    }
    else
    {
      return Result.Invalid(new ValidationError("Invalid username/email or password"));
    }

    // Generate auth response (access token + refresh token)
    var authResponse = GenerateAuthResponse(user);

    // Add new refresh token to user entity
    var refreshToken = new RefreshToken(authResponse.RefreshToken, DateTime.UtcNow.AddDays(7));
    user.AddRefreshToken(refreshToken);

    // Save the new refresh token
    await _userRepository.UpdateAsync(user);

    return Result.Success(authResponse);
  }

  public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
  {
    // Validate email is unique
    if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
      return Result.Conflict("Email is already in use");

    // Create the user with hashed password
    var passwordHash = HashPassword(request.Password);

    var emailCreateResult = Core.ValueObjects.Email.Create(request.Email);
    if (emailCreateResult.IsInvalid())
      return Result.Invalid(emailCreateResult.ValidationErrors);

    var userCreateResult = MorentUser.CreateLocalUser(request.Name, request.Username, emailCreateResult.Value, passwordHash);
    if (userCreateResult.IsInvalid())
      return Result.Invalid(userCreateResult.ValidationErrors);

    var user = userCreateResult.Value;
    await _userRepository.AddAsync(user, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Return authentication response
    var authRequest = new LoginRequest
    {
      LoginId = request.Email,
      Password = request.Password
    };

    return await AuthenticateAsync(authRequest, cancellationToken);
  }

  public async Task<Result<MorentUser>> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrEmpty(token))
      return Result.Invalid(new ValidationError("Provided token is empty!"));

    var user = await _userRepository.GetByRefreshTokenAsync(token);

    if (user == null)
      return Result.Unauthorized("No user associated with this token");

    var refreshToken = user.GetActiveRefreshToken(token);

    if (refreshToken is null || refreshToken.IsRevoked)
      return Result.Unauthorized("User has no active refresh tokens");

    return Result.Success(user);
  }
}