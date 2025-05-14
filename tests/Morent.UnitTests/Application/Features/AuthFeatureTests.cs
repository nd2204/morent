using Shouldly;
using Xunit;
using NSubstitute;
using Morent.Application.Features.Auth;
using Morent.Application.Features.Auth.DTOs;
using Morent.Application.Interfaces;
using Morent.Application.Repositories;
using Morent.Core.MorentUserAggregate;
using Morent.Core.ValueObjects;
using Ardalis.Result;
using Morent.Application.Features.User.DTOs;

namespace Morent.UnitTest.Features;

public class AuthFeatureTests
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly IOAuthService _oAuthService;

    public AuthFeatureTests()
    {
        _authService = Substitute.For<IAuthService>();
        _userRepository = Substitute.For<IUserRepository>();
        _oAuthService = Substitute.For<IOAuthService>();
    }

    [Fact]
    public async Task LoginUserQueryHandler_WithValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var query = new LoginUserQuery("test@example.com", "password123");
        var expectedResponse = new AuthResponse
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = new UserDto
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                ImageUrl = ""
            }
        };

        _authService
            .AuthenticateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<AuthResponse>.Success(expectedResponse));

        var handler = new LoginUserQueryHandler(_authService);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.AccessToken.ShouldBe(expectedResponse.AccessToken);
        result.Value.RefreshToken.ShouldBe(expectedResponse.RefreshToken);
        result.Value.User.Email.ShouldBe(expectedResponse.User.Email);
    }

    [Fact]
    public async Task RegisterUserCommandHandler_WithValidData_ShouldReturnAuthResponse()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "Test User",
            "testuser",
            "test@example.com",
            "password123"
        );

        var expectedResponse = new AuthResponse
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = new UserDto
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                ImageUrl = ""
            }
        };

        _authService
            .RegisterAsync(Arg.Any<RegisterRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<AuthResponse>.Success(expectedResponse));

        var handler = new RegisterUserCommandHandler(_userRepository, _authService);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.AccessToken.ShouldBe(expectedResponse.AccessToken);
        result.Value.RefreshToken.ShouldBe(expectedResponse.RefreshToken);
        result.Value.User.Email.ShouldBe(expectedResponse.User.Email);
    }

    [Fact]
    public async Task RefreshTokenCommandHandler_WithValidToken_ShouldReturnNewAuthResponse()
    {
        // Arrange
        var refreshToken = "valid-refresh-token";
        var command = new RefreshTokenCommand(refreshToken);
        var user = MorentUser.CreateLocalUser(
            "Test User",
            "test@example.com",
            Email.Create("test@example.com").Value,
            "hashedPassword"
        ).Value;

        var expectedResponse = new AuthResponse
        {
            AccessToken = "new-access-token",
            RefreshToken = "new-refresh-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                ImageUrl = ""
            }
        };

        _authService
            .ValidateRefreshTokenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result<MorentUser>.Success(user));

        _authService
            .GenerateAuthResponse(user)
            .Returns(expectedResponse);

        var handler = new RefreshTokenCommandHandler(_authService, _userRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.AccessToken.ShouldBe(expectedResponse.AccessToken);
        result.Value.RefreshToken.ShouldBe(expectedResponse.RefreshToken);
        result.Value.User.Email.ShouldBe(expectedResponse.User.Email);

        await _userRepository.Received(1).UpdateAsync(Arg.Any<MorentUser>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RevokeTokenCommandHandler_WithValidToken_ShouldRevokeToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshToken = "valid-refresh-token";
        var command = new RevokeTokenCommand(refreshToken, userId);
        var user = MorentUser.CreateLocalUser(
            "Test User",
            "test@example.com",
            Email.Create("test@example.com").Value,
            "hashedPassword"
        ).Value;

        _userRepository
            .GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(user);

        var handler = new RevokeTokenCommandHandler(_userRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _userRepository.Received(1).UpdateAsync(Arg.Any<MorentUser>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task OAuthLoginCommandHandler_WithValidToken_ShouldReturnAuthResponse()
    {
        // Arrange
        var provider = "Google";
        var idToken = "valid-id-token";
        var command = new OAuthLoginCommand(provider, idToken);
        var oAuthLoginInfo = new OAuthLoginInfo
        {
            ProviderKey = "google-123",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User"
        };

        var expectedResponse = new AuthResponse
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = new UserDto
            {
                UserId = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                ImageUrl = ""
            }
        };

        _oAuthService
            .ValidateExternalToken(provider, idToken)
            .Returns(oAuthLoginInfo);

        _userRepository
            .GetByOAuthInfoAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((MorentUser?)null);

        _userRepository
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((MorentUser?)null);

        _authService
            .GenerateAuthResponse(Arg.Any<MorentUser>())
            .Returns(expectedResponse);

        var handler = new OAuthLoginCommandHandler(
            _authService,
            _oAuthService,
            _userRepository
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.AccessToken.ShouldBe(expectedResponse.AccessToken);
        result.RefreshToken.ShouldBe(expectedResponse.RefreshToken);
        result.User.Email.ShouldBe(expectedResponse.User.Email);

        await _userRepository.Received(1).AddAsync(Arg.Any<MorentUser>(), Arg.Any<CancellationToken>());
    }
} 