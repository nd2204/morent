using Shouldly;
using Morent.Core.ValueObjects;
using Xunit;
using System;
using System.Linq;
using System.Threading;

namespace Morent.UnitTests.Core.ValueObjects;

public class RefreshTokenTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateRefreshToken()
    {
        // Arrange & Act
        string tokenValue = "valid-token-string";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);

        // Assert
        refreshToken.Token.ShouldBe(tokenValue);
        refreshToken.ExpiresAt.ShouldBe(expiryDate);
        refreshToken.CreatedAt.ShouldNotBe(default);
        refreshToken.RevokedAt.ShouldBeNull();
    }

    [Fact]
    public void IsRevoked_ForNonExpiredAndNonRevokedToken_ShouldReturnFalse()
    {
        // Arrange
        string tokenValue = "valid-token-string";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);

        // Act & Assert
        refreshToken.IsRevoked.ShouldBeFalse();
    }

    [Fact]
    public void IsRevoked_ForExpiredToken_ShouldReturnTrue()
    {
        // Arrange
        string tokenValue = "expired-token";
        DateTime expiryDate = DateTime.UtcNow.AddDays(-1); // Expired 1 day ago
        var refreshToken = new RefreshToken(tokenValue, expiryDate);

        // Act & Assert
        refreshToken.IsRevoked.ShouldBeTrue();
    }

    [Fact]
    public void IsRevoked_ForExplicitlyRevokedToken_ShouldReturnTrue()
    {
        // Arrange
        string tokenValue = "revoked-token";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);
        
        // Act
        refreshToken.Revoke();
        
        // Assert
        refreshToken.IsRevoked.ShouldBeTrue();
        refreshToken.RevokedAt.ShouldNotBeNull();
    }

    [Fact]
    public void IsActive_ForNonExpiredAndNonRevokedToken_ShouldReturnTrue()
    {
        // Arrange
        string tokenValue = "valid-token-string";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);

        // Act & Assert
        refreshToken.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void IsActive_ForExpiredToken_ShouldReturnFalse()
    {
        // Arrange
        string tokenValue = "expired-token";
        DateTime expiryDate = DateTime.UtcNow.AddDays(-1); // Expired 1 day ago
        var refreshToken = new RefreshToken(tokenValue, expiryDate);

        // Act & Assert
        refreshToken.IsActive.ShouldBeFalse();
    }

    [Fact]
    public void IsActive_ForExplicitlyRevokedToken_ShouldReturnFalse()
    {
        // Arrange
        string tokenValue = "revoked-token";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);
        
        // Act
        refreshToken.Revoke();
        
        // Assert
        refreshToken.IsActive.ShouldBeFalse();
    }

    [Fact]
    public void Revoke_WhenCalled_ShouldSetRevokedAtToCurrentTime()
    {
        // Arrange
        string tokenValue = "to-be-revoked-token";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);
        
        // Get time approximately before revocation
        DateTime beforeRevoke = DateTime.Now;
        
        // Act
        refreshToken.Revoke();
        
        // Assert
        refreshToken.RevokedAt.ShouldNotBeNull();
        refreshToken.RevokedAt.Value.ShouldBeGreaterThanOrEqualTo(beforeRevoke);
        refreshToken.RevokedAt.Value.ShouldBeLessThanOrEqualTo(DateTime.Now);
    }

    [Fact]
    public void Revoke_WhenCalledMultipleTimes_ShouldOnlySetRevokedAtOnce()
    {
        // Arrange
        string tokenValue = "to-be-revoked-token";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);
        
        // Act
        refreshToken.Revoke();
        DateTime? firstRevokeTime = refreshToken.RevokedAt;
        
        // Wait a small amount of time
        Thread.Sleep(50);
        
        refreshToken.Revoke(); // Try to revoke again
        
        // Assert
        refreshToken.RevokedAt.ShouldBe(firstRevokeTime); // Should not have changed
    }

    [Fact]
    public void GetEqualityComponents_ShouldIncludeAllProperties()
    {
        // Arrange
        string tokenValue = "test-token";
        DateTime expiryDate = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(tokenValue, expiryDate);
        
        // Act
        var components = refreshToken.GetType()
            .GetMethod("GetEqualityComponents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(refreshToken, null) as System.Collections.Generic.IEnumerable<object>;
        
        // Assert
        components.ShouldNotBeNull();
        components.Count().ShouldBe(4); // Token, ExpiresAt, CreatedAt, RevokedAt
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var token1 = new RefreshToken("token1", DateTime.UtcNow.AddDays(7));
        var token2 = new RefreshToken("token2", DateTime.UtcNow.AddDays(7));
        
        // Act & Assert
        token1.Equals(token2).ShouldBeFalse();
    }
} 