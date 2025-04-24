using System;
using Morent.Application.Interfaces;

namespace Morent.Infrastructure.Data;

public class EfUnitOfWork : IUnitOfWork
{
  private MorentDbContext _context;

  public EfUnitOfWork(MorentDbContext context)
  {
    _context = context;
  }

  public async Task BeginTransactionAsync()
  {
    await _context.Database.BeginTransactionAsync();
  }

  public async Task CommitTransactionAsync()
  {
    await _context.Database.BeginTransactionAsync();
  }

  public async Task RollbackTransactionAsync()
  {
    await _context.Database.RollbackTransactionAsync();
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await _context.SaveChangesAsync(cancellationToken);
  }
}