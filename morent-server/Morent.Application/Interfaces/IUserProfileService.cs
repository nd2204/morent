using System;

namespace Morent.Application.Interfaces;

public interface IUserProfileService
{
  Task<UserProfileImageDto> GetUserProfileImageAsync(Guid userId);
  Task<UserProfileImageDto> UpdateUserProfileImageAsync(Guid userId, ImageUploadRequest imageUpload);
  Task<bool> RemoveUserProfileImageAsync(Guid userId);
}
