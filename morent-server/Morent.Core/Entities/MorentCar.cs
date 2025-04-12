using System.ComponentModel.DataAnnotations.Schema;

namespace Morent.Core.Entities;

public class MorentCar
{
  public int Id { get; set; }
  public string PlateNumber { get; set; } = null!;
  public string Status { get; set; } = null!;

  public int CarModelId { get; set; }
  [ForeignKey(nameof(CarModelId))]
  public MorentCarModel CarModel { get; set; } = null!;

  public ICollection<MorentRental> Rentals { get; set; } = new List<MorentRental>();
  public ICollection<MorentReview> Reviews { get; set; } = new List<MorentReview>();
  public ICollection<MorentFavorite> Favorites { get; set; } = new List<MorentFavorite>();
}