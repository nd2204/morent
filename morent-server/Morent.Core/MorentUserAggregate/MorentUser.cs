using Morent.Core.MorentUserAggregate.Events;

namespace Morent.Core.MorentUserAggregate;

public class MorentUser : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Username { get; private set; }
    public string? PasswordHash { get; private set; }
    public MorentUserRole Role { get; private set; }
    public Guid? ProfileImageId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private readonly List<MorentUserOAuthLogin> _OAuthLogins = new();
    public IReadOnlyCollection<MorentUserOAuthLogin> OAuthLogins => _OAuthLogins.AsReadOnly();

    // For EF Core
    private MorentUser() { }

    // For local authentication
    private MorentUser(
        string? name, string username, Email email, string? passwordHash = null, MorentUserRole role = MorentUserRole.Customer)
    {
        Id = Guid.NewGuid();
        Username = username;
        Name = name ?? username;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    private static Result<MorentUser> Create(string? name, string username, Email email, string? passwordHash = null, MorentUserRole role = MorentUserRole.Customer)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Invalid(new ValidationError("User", "Username cannot be empty."));

        var user = new MorentUser(name, username, email, passwordHash, role);

        return Result.Success(user);

    }

    // Factory method for local user creation
    public static Result<MorentUser> CreateLocalUser(string? name, string username, Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Invalid(new ValidationError("User", "Password hash cannot be empty."));

        var createResult = Create(name, username, email, passwordHash);
        if (createResult.IsSuccess)
            createResult.Value.RegisterDomainEvent(new UserCreatedEvent(createResult.Value.Id));

        return createResult;
    }

    public static Result<MorentUser> CreateExternalUser(string? name, string username, Email email, string provider, string providerKey)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return Result.Invalid(new ValidationError("User", "Provider cannot be empty."));

        if (string.IsNullOrWhiteSpace(providerKey))
            return Result.Invalid(new ValidationError("User", "Provider Key cannot be empty."));

        var createResult = Create(name, username, email);

        if (createResult.IsSuccess)
        {
            var user = createResult.Value;
            user.AddExternalLogin(provider, providerKey);
            user.RegisterDomainEvent(new UserCreatedEvent(user.Id));
        }

        return createResult;
    }

    public static Result<MorentUser> CreateAdmin(string? name, string username, Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Invalid(new ValidationError("User", "Password hash cannot be empty."));

        var createResult = Create(name, username, email, passwordHash, MorentUserRole.Admin);
        return createResult;
    }

    public Result UpdateProfile(string name, Guid? profileImageId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Invalid(new ValidationError("User", "Name cannot be empty."));

        Name = name;
        ProfileImageId = profileImageId;
        //   UpdateModifiedDate();

        return Result.Success();
    }

    public void SetPrimaryImage(Guid? imageId)
    {
        ProfileImageId = imageId;
    }

    public void RemoveProfileImage()
    {
        ProfileImageId = null;
    }

    public Result ChangePassword(string currentPasswordHash, string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            return Result.Invalid(new ValidationError("User", "New password hash cannot be empty."));

        if (currentPasswordHash != PasswordHash)
            return Result.Invalid(new ValidationError("User", "Current password is incorrect."));

        PasswordHash = newPasswordHash;
        //   UpdateModifiedDate();

        return Result.Success();
    }

    public void UpdateEmail(Email email)
    {
        Email = email;
    }

    public void SetRole(MorentUserRole role)
    {
        Role = role;
        //   UpdateModifiedDate();
    }

    public void Deactivate()
    {
        IsActive = false;
        //   UpdateModifiedDate();
        this.RegisterDomainEvent(new UserDeactivatedEvent(Id));
    }

    public void Activate()
    {
        IsActive = true;
        //   UpdateModifiedDate();
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        //   UpdateModifiedDate();
    }


    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Add(refreshToken);
    }

    public RefreshToken? GetActiveRefreshToken(string token)
    {
        return _refreshTokens
            .FirstOrDefault(rt => rt.Token == token && rt.IsActive);
    }

    public void RevokeAllRefreshTokens()
    {
        foreach (var token in _refreshTokens.Where(t => t.IsActive))
        {
            token.Revoke();
        }
    }

    public void RevokeRefreshToken(string token)
    {
        var refreshToken = _refreshTokens.FirstOrDefault(rt => rt.Token == token);
        refreshToken?.Revoke();
    }

    public void AddExternalLogin(string provider, string providerKey)
    {
        if (_OAuthLogins.Any(x => x.Provider == provider && x.ProviderKey == providerKey))
            return;

        _OAuthLogins.Add(new MorentUserOAuthLogin(provider, providerKey));
    }
}