using System;
using Ardalis.Specification.EntityFrameworkCore;

namespace Morent.Infrastructure.Data;

public class EFRepository<T>(MorentDbContext context)
  : RepositoryBase<T>(context), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
}