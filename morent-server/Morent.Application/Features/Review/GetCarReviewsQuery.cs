using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Morent.Application.Features.DTOs;

namespace Morent.Application.Features.Review;

public record class GetCarReviewsQuery : IQuery<PagedResult<IEnumerable<ReviewDto>>>
{
  public GetCarReviewsQuery(Guid carId)
  {
    CarId = carId;
  }

  public GetCarReviewsQuery(Guid carId, PagedQuery pagedQuery)
  {
    CarId = carId;
    PagedQuery = pagedQuery;
  }

  public Guid CarId { get; set; }
  public PagedQuery PagedQuery = new PagedQuery();
}