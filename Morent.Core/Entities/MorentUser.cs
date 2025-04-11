namespace Morent.Core.Entities;

public class MorentUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "user"; // "admin" or "user"

    public ICollection<MorentRental> Rentals { get; set; } = new List<MorentRental>();
    public ICollection<MorentReview> Reviews { get; set; } = new List<MorentReview>();
    public ICollection<MorentFavorite> Favorites { get; set; } = new List<MorentFavorite>();
}
