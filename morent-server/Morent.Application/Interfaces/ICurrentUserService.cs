namespace Morent.Application.Interfaces;

public interface ICurrentUserService
{
  Guid? UserId { get; }
  string Email { get; }
  MorentUserRole Role { get; }
  bool IsAuthenticated { get; }
}
