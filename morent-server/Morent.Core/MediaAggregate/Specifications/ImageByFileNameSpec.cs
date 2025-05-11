using System;

namespace Morent.Core.MediaAggregate.Specifications;

public class ImageByFileNameSpec : Specification<MorentImage>
{
  public ImageByFileNameSpec(string fileName) {
    Query.Where(image => image.FileName == fileName);
  }
}
