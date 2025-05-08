using System;
using Microsoft.AspNetCore.Http;

namespace Morent.Application.Features.User.Commands;

public record class UploadUserProfileImageCommand(
  Guid UserId,
  IFormFile File)
 : ICommand<Result<UserProfileImageDto>>;
