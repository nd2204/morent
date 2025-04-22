// using System;
// using Morent.Application.Interfaces;

// namespace Morent.Infrastructure.Data;

// public class EfUnitOfWork : IUnitOfWork
// {
//   private readonly DbContext _context;
//   private readonly Dictionary<Type, object> _repositories;
//   private MorentDbContext _transaction;

//   public EfUnitOfWork(DbContext context)
//   {
//     _context = context;
//     _repositories = new Dictionary<Type, object>();
//   }

//   public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
//   {
//     var type = typeof(TEntity);
//     if (!_repositories.ContainsKey(type))
//     {
//       _repositories[type] = new EfRepository<TEntity>(_context);
//     }
//     return (IRepository<TEntity>)_repositories[type];
//   }

//   public async Task<int> SaveChangesAsync()
//   {
//     return await _context.SaveChangesAsync();
//   }

//   public void BeginTransaction()
//   {
//     _transaction = _context.Database.BeginTransaction();
//   }

//   public async Task CommitTransactionAsync()
//   {
//     await _transaction.CommitAsync();
//   }

//   public void RollbackTransaction()
//   {
//     _transaction?.Rollback();
//   }

//   public void Dispose()
//   {
//     _transaction?.Dispose();
//     _context.Dispose();
//   }
// }