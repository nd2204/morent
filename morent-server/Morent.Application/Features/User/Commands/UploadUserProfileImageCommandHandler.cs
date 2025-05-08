using System;

namespace Morent.Application.Features.User.Commands;

public class UploadUserProfileImageCommandHandler(
  IUserProfileService _userProfileService)
  : ICommandHandler<UploadUserProfileImageCommand, Result<UserProfileImageDto>>
{
  public async Task<Result<UserProfileImageDto>> Handle(UploadUserProfileImageCommand request, CancellationToken cancellationToken)
  {
    try
    {
      using var stream = request.File.OpenReadStream();
      var uploadRequest = new ImageUploadRequest
      {
        ImageData = stream,
        FileName = request.File.FileName,
        ContentType = request.File.ContentType
      };

      var result = await _userProfileService.UpdateUserProfileImageAsync(request.UserId, uploadRequest);
      return result;
    }
    catch (Exception ex)
    {
      return Result.Error(ex.Message);
    }
  }
}
