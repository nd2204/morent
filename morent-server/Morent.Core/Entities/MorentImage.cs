// using Morent.Core.MorentCarAggregate;
// using Morent.Core.MorentUserAggregate;

// namespace Morent.Core.Entities;

// public class MorentImage : EntityBase<Guid>
// {
//   private MorentImage() {}

//   public MorentImage(string fileName, string url)
//   {
//     Id = Guid.NewGuid();
//     FileName = fileName;
//     Url = url;
//     UploadedAt = DateTime.UtcNow;
//   }

//   public string FileName { get; private set; } = string.Empty;
//   public string Url { get; private set; } = string.Empty;
//   public DateTime UploadedAt { get; private set; }

//   // Car image (optional)
//   public int? CarId { get; private set; }
//   public MorentCar? Car { get; private set; }

//   // User profile image (optional)
//   public Guid? UserId { get; private set; }
//   public MorentUser? User { get; private set; }

//   public void AssignToCar(MorentCar car)
//   {
//     Guard.Against.Null(car);
//     Car = car;
//     CarId = car.Id;
//   }

//   public void AssignToUser(MorentUser user)
//   {
//     Guard.Against.Null(user);
//     User = user;
//     UserId = user.Id;
//   }
// }