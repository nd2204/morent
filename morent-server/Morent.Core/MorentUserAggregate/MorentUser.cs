namespace Morent.Core.MorentUserAggregate;

public class MorentUser : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Username { get; private set; }
    public string? PasswordHash { get; private set; }
    public string? OAuthId { get; private set; }
    public OAuthProvider AuthProvider { get; private set; }
    public MorentUserRole Role { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    // For EF Core
    private MorentUser() { }

    // For local authentication
    public MorentUser(
        string? name,
        string username,
        Email email,
        string passwordHash,
        MorentUserRole role = MorentUserRole.Customer)
    {
        Guard.Against.NullOrEmpty(username, nameof(username));
        Guard.Against.NullOrEmpty(passwordHash, nameof(passwordHash));
        Id = Guid.NewGuid();
        Username = username;
        Name = name ?? username;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        AuthProvider = OAuthProvider.Local;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    // For OAuth authentication
    public MorentUser(
        string name,
        Email email,
        OAuthProvider provider,
        string oauthId,
        string profileImageUrl = null,
        MorentUserRole role = MorentUserRole.Customer)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.NullOrEmpty(oauthId, nameof(oauthId));
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        AuthProvider = provider;
        OAuthId = oauthId ?? throw new ArgumentNullException(nameof(oauthId));
        ProfileImageUrl = profileImageUrl;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    // Factory method for local user creation
    public static Result<MorentUser> CreateLocalUser(string? name, string username, Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Invalid(new ValidationError("User", "Username cannot be empty."));

        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Invalid(new ValidationError("User", "Password hash cannot be empty."));

        var user = new MorentUser(name, username, email, passwordHash);
        user.PasswordHash = passwordHash;

        user.RegisterDomainEvent(new UserCreatedEvent(user.Id));

        return Result.Success(user);
    }

    // Factory method for external provider user creation
    public static Result<MorentUser> CreateExternalUser(string name, Email email, OAuthProvider provider, string externalProviderId)
    {
        if (string.IsNullOrWhiteSpace(externalProviderId))
            return Result.Invalid(new ValidationError("User", "External provider ID cannot be empty."));

        if (provider == OAuthProvider.Local)
            return Result.Invalid(new ValidationError("User", "Invalid auth provider for external user."));

        var user = new MorentUser(Guid.NewGuid().ToString(), email, provider, externalProviderId);
        user.RegisterDomainEvent(new UserCreatedEvent(user.Id));
        return Result.Success(user);
    }

    public Result UpdateProfile(string name, string? profileImageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Invalid(new ValidationError("User", "Name cannot be empty."));

        Name = name;
        ProfileImageUrl = profileImageUrl;
        //   UpdateModifiedDate();

        return Result.Success();
    }

    public Result ChangePassword(string currentPasswordHash, string newPasswordHash)
    {
        if (AuthProvider != OAuthProvider.Local)
            return Result.Error("Cannot change password for externally authenticated users.");

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
}