using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ardalis.Result;
using Microsoft.Extensions.Options;
using Morent.Application.Interfaces;
using Morent.Application.DTOs;
using BCrypt.Net;
using Morent.Core.ValueObjects;
using Morent.Infrastructure.Settings;
using Morent.Application.Repositories;

namespace Morent.Infrastructure.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IOAuthService _oAuthService;
  private readonly AppSettings _appSettings;
  private readonly IHttpClientFactory _httpClientFactory;

  public AuthService(
    IUserRepository userRepository,
    IOptions<AppSettings> appSettings,
    IOAuthService oAuthService,
    IHttpClientFactory httpClientFactory)
  {
    _userRepository = userRepository;
    _appSettings = appSettings.Value;
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
        new Claim(JwtRegisteredClaimNames.Sub, user.Name),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

  public async Task<AuthResponse> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default)
  {
    // Find user by email
    var user = await _userRepository.GetByUsernameOrEmail(request.LoginId);
    if (user == null)
      throw new UnauthorizedAccessException("Invalid email or password");

    // Validate password
    bool isPasswordValid = VerifyPassword(request.Password, user.PasswordHash!);
    if (!isPasswordValid)
      throw new UnauthorizedAccessException("Invalid email or password");

    // Generate auth response (access token + refresh token)
    var authResponse = GenerateAuthResponse(user);

    // Add new refresh token to user entity
    var refreshToken = new RefreshToken(authResponse.RefreshToken, DateTime.UtcNow.AddDays(7));
    user.AddRefreshToken(refreshToken);

    // Save the new refresh token
    await _userRepository.UpdateAsync(user);

    return authResponse;
  }

  public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
  {
    // Validate email is unique
    if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
      throw new ApplicationException("Email is already in use");

    // Create the user with hashed password
    var userId = Guid.NewGuid();
    var passwordHash = HashPassword(request.Password);
    var email = new Email(request.Email);

    var user = new MorentUser(request.Name, request.Username, email, passwordHash);
    await _userRepository.AddAsync(user, cancellationToken);
    await _userRepository.SaveChangesAsync();
    // await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Return authentication response
    var authRequest = new LoginRequest
    {
      LoginId = request.Email,
      Password = request.Password
    };

    return await AuthenticateAsync(authRequest, cancellationToken);
  }

  public async Task<MorentUser?> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrEmpty(token))
      return null;

    var user = await _userRepository.GetByRefreshTokenAsync(token);

    if (user == null)
      return null;

    var refreshToken = user.GetActiveRefreshToken(token);

    if (refreshToken is null || refreshToken.IsRevoked)
      return null;

    return user;
  }

  public Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}