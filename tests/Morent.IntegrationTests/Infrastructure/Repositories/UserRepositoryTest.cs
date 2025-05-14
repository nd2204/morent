using System;

namespace Morent.IntegrationTests.Infrastructure.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;
using Morent.Application.Repositories;
using Morent.Core.MorentUserAggregate;
using Morent.Core.ValueObjects;
using Morent.IntegrationTests.Data;
using Shouldly;
using Xunit;

public class UserRepositoryIntegrationTests : BaseEfRepoTestFixture, IDisposable
{
  private readonly IUserRepository _userRepository;

  public UserRepositoryIntegrationTests()
  {
    _userRepository = GetUserRepository();
    SeedTestData().GetAwaiter().GetResult();
  }

  private async Task SeedTestData()
  {
    // Create test users
    var users = new List<MorentUser>
    {
      MorentUser.CreateExternalUser(
        name: null,
        username: "testuser1",
        email: Email.Create("test1@example.com"),
        provider: "google",
        providerKey: "google-key-1"
      ),
      MorentUser.CreateExternalUser(
        name: null,
        username: "testuser2",
        email: Email.Create("test2@example.com"),
        provider: "facebook",
        providerKey: "facebook-key-1"
      )
    };

    users[0].AddRefreshToken(new RefreshToken("refresh-token-1", DateTime.Now.AddDays(7)));
    users[1].AddRefreshToken(new RefreshToken("refresh-token-2", DateTime.Now.AddDays(7)));

    await _dbContext.Users.AddRangeAsync(users);
    await _dbContext.SaveChangesAsync();
  }

  [Fact]
  public async Task GetByEmailAsync_WithExistingEmail_ReturnsUser()
  {
    // Arrange
    var email = Email.Create("test1@example.com");
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByEmailAsync(email.Value.ToString(), cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.Email.Value.ShouldBe(email.Value.ToString());
  }

  [Fact]
  public async Task GetByEmailAsync_WithNonExistingEmail_ReturnsNull()
  {
    // Arrange
    var email = "nonexistent@example.com";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByEmailAsync(email, cancellationToken);

    // Assert
    result.ShouldBeNull();
  }

  [Fact]
  public async Task GetByUsername_WithExistingUsername_ReturnsUser()
  {
    // Arrange
    var username = "testuser1";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByUsername(username, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.Username.ShouldBe(username);
  }

  [Fact]
  public async Task GetByUsername_WithNonExistingUsername_ReturnsNull()
  {
    // Arrange
    var username = "nonexistentuser";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByUsername(username, cancellationToken);

    // Assert
    result.ShouldBeNull();
  }

  [Fact]
  public async Task GetByUsernameOrEmail_WithExistingUsername_ReturnsUser()
  {
    // Arrange
    var username = "testuser1";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByUsernameOrEmail(username, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.Username.ShouldBe(username);
  }

  [Fact]
  public async Task GetByUsernameOrEmail_WithExistingEmail_ReturnsUser()
  {
    // Arrange
    var email = "test2@example.com";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByUsernameOrEmail(email, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.Email.ToString().ShouldBe(email);
  }

  [Fact]
  public async Task GetByUsernameOrEmail_WithNonExistingInput_ReturnsNull()
  {
    // Arrange
    var input = "nonexistent";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByUsernameOrEmail(input, cancellationToken);

    // Assert
    result.ShouldBeNull();
  }

  [Fact]
  public async Task GetByOAuthInfoAsync_WithExistingProvider_ReturnsUser()
  {
    // Arrange
    var provider = "google";
    var providerKey = "google-key-1";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByOAuthInfoAsync(provider, providerKey, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    var oauthLogin = result.OAuthLogins.First();
    oauthLogin.ShouldNotBeNull();
    oauthLogin.Provider.ShouldBe(provider);
    oauthLogin.ProviderKey.ShouldBe(providerKey);
  }

  [Fact]
  public async Task GetByOAuthInfoAsync_WithNonExistingProviderInfo_ReturnsNull()
  {
    // Arrange
    var provider = "twitter";
    var providerKey = "non-existent-key";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByOAuthInfoAsync(provider, providerKey, cancellationToken);

    // Assert
    result.ShouldBeNull();
  }

  [Fact]
  public async Task GetByRefreshTokenAsync_WithExistingToken_ReturnsUser()
  {
    // Arrange
    var refreshToken = "refresh-token-1";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);

    // Assert
    result.ShouldNotBeNull();
    result.RefreshTokens.First().Token.ShouldBe(refreshToken);
  }

  [Fact]
  public async Task GetByRefreshTokenAsync_WithNonExistingToken_ReturnsNull()
  {
    // Arrange
    var refreshToken = "non-existent-token";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);

    // Assert
    result.ShouldBeNull();
  }

  [Fact]
  public async Task ExistsByEmailAsync_WithExistingEmail_ReturnsTrue()
  {
    // Arrange
    var email = "test1@example.com";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.ExistsByEmailAsync(email, cancellationToken);

    // Assert
    result.ShouldBeTrue();
  }

  [Fact]
  public async Task ExistsByEmailAsync_WithNonExistingEmail_ReturnsFalse()
  {
    // Arrange
    var email = "nonexistent@example.com";
    var cancellationToken = CancellationToken.None;

    // Act
    var result = await _userRepository.ExistsByEmailAsync(email, cancellationToken);

    // Assert
    result.ShouldBeFalse();
  }

  public void Dispose()
  {
    // Clean up the in-memory database after each test
    _dbContext.Database.EnsureDeleted();
    _dbContext.Dispose();
  }
}