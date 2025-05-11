using System;

namespace Morent.Core.MediaAggregate.Specifications;

public class ImageByPathSpec : Specification<MorentImage>
{
  public ImageByPathSpec(string path)
  {
    Query.Where(image => image.Path == path);
  }
}
