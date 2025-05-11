using System;

namespace Morent.Application.Extensions;

public static class ImageExtension
{
  public static CarImageDto ToCarImageDto(this ImageDto image)
  {
    return new CarImageDto {
      ImageId = image.Id,
      Url = image.Url
    };
  }
}
