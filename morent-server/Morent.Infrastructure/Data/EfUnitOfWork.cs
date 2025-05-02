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
    await _context.Database.CommitTransactionAsync();
  }

  public async Task RollbackTransactionAsync()
  {
    if (_context.Database.CurrentTransaction != null)
    {
      await _context.Database.RollbackTransactionAsync();
    }
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    var entries = _context.ChangeTracker.Entries();
    foreach (var entry in entries)
    {
      Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
    }
    return await _context.SaveChangesAsync(cancellationToken);
  }
}