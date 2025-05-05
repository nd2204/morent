using System;
using Morent.Application.Features.Auth.DTOs;
using Morent.Application.Features.Images.DTOs;
using Morent.Application.Interfaces;
using Morent.Application.Repositories;
using Morent.Core.MediaAggregate;

namespace Morent.Infrastructure.Services;

public class UserProfileService : IUserProfileService
{
  private readonly IUserRepository _userRepository;
  private readonly IRepository<MorentImage> _imageRepository;
  private readonly IImageService _imageService;
  private readonly IUnitOfWork _unitOfWork;

  public UserProfileService(
      IUserRepository userRepository,
      IRepository<MorentImage> imageRepository,
      IImageService imageService,
      IUnitOfWork unitOfWork)
  {
    _userRepository = userRepository;
    _imageRepository = imageRepository;
    _imageService = imageService;
    _unitOfWork = unitOfWork;
  }

  public async Task<Result<UserProfileImageDto>> GetUserProfileImageAsync(Guid userId)
  {
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null || !user.ProfileImageId.HasValue)
      return Result.NotFound($"User with ID {userId} not found or doesn't have an profile image");

    var result = await _imageService.GetImageByIdAsync(user.ProfileImageId.Value);
    if (!result.IsSuccess)
      return Result.Error(new ErrorList(result.Errors));

    var imageResult = result.Value;

    return Result.Success(new UserProfileImageDto
    {
      ImageId = imageResult.Id,
      Url = imageResult.Url,
      UploadedAt = imageResult.UploadedAt
    });
  }

  public async Task<Result<UserProfileImageDto>> UpdateUserProfileImageAsync(Guid userId, ImageUploadRequest imageUpload)
  {
    if (imageUpload.ImageData.Length > 2 * 1024 * 1024) // 2MB limit for profile images
      return Result.Invalid(new ValidationError("Image", "Profile image exceeds maximum size of 2MB"));

    await _unitOfWork.BeginTransactionAsync();

    // Get user
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null)
    {
      await _unitOfWork.RollbackTransactionAsync();
      throw new ApplicationException($"User with ID {userId} not found");
    }

    // Store old image ID to delete later if exists
    var oldImageId = user.ProfileImageId;

    // Upload new image
    var uploadResult = await _imageService.UploadImageAsync(imageUpload);
    if (!uploadResult.IsSuccess)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.Error($"Failed to upload image: {string.Join(", ", uploadResult.Errors)}");
    }

    var uploadResponse = uploadResult.Value;

    // Update user profile image
    user.SetPrimaryImage(uploadResult.Value.ImageId);
    await _userRepository.UpdateAsync(user);
    await _unitOfWork.CommitTransactionAsync();

    // Delete old image in background if exists
    if (oldImageId.HasValue)
    {
      // Note: In a real system, this would be handled by a background job
      // to avoid transaction issues if the delete fails
      _ = Task.Run(async () =>
      {
        await _imageService.DeleteImageAsync(oldImageId.Value);
      });
    }

    // Return result
    return new UserProfileImageDto
    {
      ImageId = uploadResponse.ImageId,
      Url = uploadResponse.Url,
      UploadedAt = DateTime.UtcNow
    };
  }

  public async Task<Result> RemoveUserProfileImageAsync(Guid userId)
  {
    await _unitOfWork.BeginTransactionAsync();

    // Get user
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null || !user.ProfileImageId.HasValue)
    {
      await _unitOfWork.RollbackTransactionAsync();
      return Result.NotFound($"User with ID {userId} not found or doesn't have an profile image");
    }

    // Store image ID to delete
    var imageId = user.ProfileImageId.Value;

    // Remove profile image reference
    user.RemoveProfileImage();
    await _userRepository.UpdateAsync(user);

    // Delete the image
    await _imageService.DeleteImageAsync(imageId);

    await _unitOfWork.CommitTransactionAsync();
    return Result.Success();
  }
}