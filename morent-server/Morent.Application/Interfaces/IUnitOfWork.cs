using System;

namespace Morent.Application.Interfaces;

public interface IUnitOfWork
{
  // IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IAggregateRoot;
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  Task BeginTransactionAsync();
  Task CommitTransactionAsync();
  Task RollbackTransactionAsync();
}
