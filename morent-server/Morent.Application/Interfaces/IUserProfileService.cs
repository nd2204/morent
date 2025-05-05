using System;

namespace Morent.Application.Interfaces;

public interface IUserProfileService
{
  Task<Result<UserProfileImageDto>> GetUserProfileImageAsync(Guid userId);
  Task<Result<UserProfileImageDto>> UpdateUserProfileImageAsync(Guid userId, ImageUploadRequest imageUpload);
  Task<Result> RemoveUserProfileImageAsync(Guid userId);
}
