using Shouldly;
using Morent.Core.Exceptions;
using Morent.Core.MorentUserAggregate;
using Morent.Core.ValueObjects;
using Ardalis.Result;
using Xunit;

namespace Morent.UnitTests.Core.MorentUserAggregate;

public class MorentUserTests
{
    private readonly string _name = "Test User";
    private readonly string _username = "testuser";
    private readonly Email _email;
    private readonly string _passwordHash = "hashed_password";

    private readonly string _provider = "exampleProvider";
    private readonly string _providerKey = "exampleKey";

    public MorentUserTests()
    {
        _email = Email.Create("test@example.com").Value;
    }

    [Fact]
    public void CreateLocalUser_WithValidParameters_ShouldCreateUser()
    {
        // Arrange & Act
        var result = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var user = result.Value;
        user.Name.ShouldBe(_name);
        user.Username.ShouldBe(_username);
        user.Email.ShouldBe(_email);
        user.PasswordHash.ShouldBe(_passwordHash);
        user.Role.ShouldBe(MorentUserRole.Customer);
        user.IsActive.ShouldBeTrue();
        user.CreatedAt.ShouldNotBe(default);
        user.LastLoginAt.ShouldBeNull();
        user.ProfileImageId.ShouldBeNull();
    }

    [Theory]
    [InlineData("Test User", "", "test@example.com", "password")]
    [InlineData("Test User", "testuser", "test@example.com", "")]
    public void CreateLocalUser_WithInvalidParameters_ShouldReturnValidationError(string? name, string username, string email, string passwordHash)
    {
        // Arrange
        var emailValue = Email.Create(email).Value;

        // Act
        var result = MorentUser.CreateLocalUser(name, username, emailValue, passwordHash);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void CreateExternalUser_WithValidParameters_ShouldCreateUser()
    {
        // Arrange & Act
        var result = MorentUser.CreateExternalUser(_name, _username, _email, _provider, _providerKey);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var user = result.Value;
        var oauth = user.OAuthLogins.First();
        oauth.Provider.ShouldBe(_provider);
        oauth.ProviderKey.ShouldBe(_providerKey);

        user.Name.ShouldBe(_name);
        user.Username.ShouldBe(_username);
        user.Email.ShouldBe(_email);
        user.PasswordHash.ShouldBeNull();
        user.Role.ShouldBe(MorentUserRole.Customer);
        user.IsActive.ShouldBeTrue();
    }

    [Theory]
    [InlineData("Test User", "", "test@example.com")]
    public void CreateExternalUser_WithInvalidParameters_ShouldReturnValidationError(string? name, string username, string email)
    {
        // Arrange
        var emailValue = Email.Create(email).Value;

        // Act
        var result = MorentUser.CreateExternalUser(name, username, emailValue, _provider, _providerKey);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void CreateAdmin_WithValidParameters_ShouldCreateAdminUser()
    {
        // Arrange & Act
        var result = MorentUser.CreateAdmin(_name, _username, _email, _passwordHash);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var user = result.Value;
        user.Name.ShouldBe(_name);
        user.Username.ShouldBe(_username);
        user.Email.ShouldBe(_email);
        user.PasswordHash.ShouldBe(_passwordHash);
        user.Role.ShouldBe(MorentUserRole.Admin);
        user.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void UpdateProfile_WithValidParameters_ShouldUpdateProfile()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var newName = "Updated Name";
        var newImageId = Guid.NewGuid();

        // Act
        var result = user.UpdateProfile(newName, newImageId);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        user.Name.ShouldBe(newName);
        user.ProfileImageId.ShouldBe(newImageId);
    }

    [Fact]
    public void UpdateProfile_WithEmptyName_ShouldReturnValidationError()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;

        // Act
        var result = user.UpdateProfile("", Guid.NewGuid());

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void ChangePassword_WithValidParameters_ShouldChangePassword()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var newPasswordHash = "new_hashed_password";

        // Act
        var result = user.ChangePassword(_passwordHash, newPasswordHash);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        user.PasswordHash.ShouldBe(newPasswordHash);
    }

    [Fact]
    public void ChangePassword_WithIncorrectCurrentPassword_ShouldReturnValidationError()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var incorrectPasswordHash = "incorrect_hash";
        var newPasswordHash = "new_hashed_password";

        // Act
        var result = user.ChangePassword(incorrectPasswordHash, newPasswordHash);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void UpdateEmail_ShouldUpdateEmail()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var newEmail = Email.Create("new@example.com").Value;

        // Act
        user.UpdateEmail(newEmail);

        // Assert
        user.Email.ShouldBe(newEmail);
    }

    [Fact]
    public void SetRole_ShouldUpdateRole()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;

        // Act
        user.SetRole(MorentUserRole.Admin);

        // Assert
        user.Role.ShouldBe(MorentUserRole.Admin);
    }

    [Fact]
    public void Deactivate_ShouldDeactivateUser()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.ShouldBeFalse();
    }

    [Fact]
    public void Activate_ShouldActivateUser()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        user.Deactivate();

        // Act
        user.Activate();

        // Assert
        user.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void RecordLogin_ShouldUpdateLastLoginAt()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;

        // Act
        user.RecordLogin();

        // Assert
        user.LastLoginAt.ShouldNotBeNull();
        user.LastLoginAt!.Value.ShouldBeGreaterThanOrEqualTo(DateTime.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public void AddRefreshToken_ShouldAddTokenToCollection()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var refreshToken = new RefreshToken("token", DateTime.UtcNow.AddDays(7));

        // Act
        user.AddRefreshToken(refreshToken);

        // Assert
        user.RefreshTokens.ShouldContain(refreshToken);
    }

    [Fact]
    public void GetActiveRefreshToken_WithValidToken_ShouldReturnToken()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var refreshToken = new RefreshToken("token", DateTime.UtcNow.AddDays(7));
        user.AddRefreshToken(refreshToken);

        // Act
        var result = user.GetActiveRefreshToken("token");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(refreshToken);
    }

    [Fact]
    public void GetActiveRefreshToken_WithExpiredToken_ShouldReturnNull()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var refreshToken = new RefreshToken("token", DateTime.UtcNow.AddDays(-1));
        user.AddRefreshToken(refreshToken);

        // Act
        var result = user.GetActiveRefreshToken("token");

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void RevokeAllRefreshTokens_ShouldRevokeAllActiveTokens()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var token1 = new RefreshToken("token1", DateTime.UtcNow.AddDays(7));
        var token2 = new RefreshToken("token2", DateTime.UtcNow.AddDays(7));
        user.AddRefreshToken(token1);
        user.AddRefreshToken(token2);

        // Act
        user.RevokeAllRefreshTokens();

        // Assert
        token1.IsActive.ShouldBeFalse();
        token2.IsActive.ShouldBeFalse();
    }

    [Fact]
    public void RevokeRefreshToken_ShouldRevokeSpecificToken()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var token = new RefreshToken("token", DateTime.UtcNow.AddDays(7));
        user.AddRefreshToken(token);

        // Act
        user.RevokeRefreshToken("token");

        // Assert
        token.IsActive.ShouldBeFalse();
    }

    [Fact]
    public void AddExternalLogin_ShouldAddOAuthLogin()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var provider = "Google";
        var providerKey = "google123";

        // Act
        user.AddExternalLogin(provider, providerKey);

        // Assert
        user.OAuthLogins.ShouldContain(x => x.Provider == provider && x.ProviderKey == providerKey);
    }

    [Fact]
    public void AddExternalLogin_WithDuplicateProviderAndKey_ShouldNotAddDuplicate()
    {
        // Arrange
        var user = MorentUser.CreateLocalUser(_name, _username, _email, _passwordHash).Value;
        var provider = "Google";
        var providerKey = "google123";
        user.AddExternalLogin(provider, providerKey);

        // Act
        user.AddExternalLogin(provider, providerKey);

        // Assert
        user.OAuthLogins.Count(x => x.Provider == provider && x.ProviderKey == providerKey).ShouldBe(1);
    }
}