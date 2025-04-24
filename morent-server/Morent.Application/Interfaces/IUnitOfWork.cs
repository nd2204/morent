using System;

namespace Morent.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
  IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IAggregateRoot;
  Task<int> SaveChangesAsync();
  void BeginTransaction();
  Task CommitTransactionAsync();
  void RollbackTransaction();
}
