namespace Morent.Application.Features.DTOs;

public record class PagedQuery(
  int Page = 1, int PageSize = 10
);
