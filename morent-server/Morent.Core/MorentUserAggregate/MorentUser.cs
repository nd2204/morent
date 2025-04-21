using Morent.Core.MorentRentalAggregate;

namespace Morent.Core.MorentUserAggregate;

public class MorentUser : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public string Role { get; private set; } = null!; // "admin" or "user"
    public string? Phone { get; private set; }
    public DateTime? CreatedAt { get; private set; }

    public MorentUser(string name, string username, string email, string passwordHash, byte[] passwordSalt, string? phone)
    {
        Name = name;
        Username = Guard.Against.Null(username);
        Email = Guard.Against.Null(email);
        PasswordHash = Guard.Against.Null(passwordHash);
        PasswordSalt = Guard.Against.Null(passwordSalt);
        Phone = phone;
        Role = MorentUserRole.User.ToString();
        CreatedAt = DateTime.UtcNow;
    }

    public void SetRole(MorentUserRole role) {
        Guard.Against.Null(role);
        Role = role.ToString();
    }
    // private MorentImage? _profileImage;
    // public MorentImage? ProfileImage => _profileImage;

    // private readonly List<MorentFavorite> _favorites = new();
    // public IEnumerable<MorentFavorite> Favorites => _favorites.AsReadOnly();

    // private readonly List<MorentRental> _rental = new();
    // public IEnumerable<MorentRental> Rentals => _rental.AsReadOnly();

    // public void AddFavorite(MorentFavorite favorite)
    // {
    //     Guard.Against.Null(favorite, nameof(favorite));
    //     _favorites.Add(favorite);
    // }

    // public void AddRental(MorentRental rental)
    // {
    //     Guard.Against.Null(rental, nameof(rental));
    //     _rental.Add(rental);
    // }

    // public void UpdateProfileImage(MorentImage image)
    // {
    //     Guard.Against.Null(image, nameof(image));
    //     _profileImage = image;
    // }
}
