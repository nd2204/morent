using System;
using Microsoft.EntityFrameworkCore.Storage;
using Morent.Application.Interfaces;

namespace Morent.Infrastructure.Data;

public class EfUnitOfWork : IUnitOfWork
{
  private MorentDbContext _context;
  private IDbContextTransaction _transaction;

  public EfUnitOfWork(MorentDbContext context)
  {

    _context = context;
  }

  public async Task BeginTransactionAsync()
  {
    if (_transaction == null && _context.Database.CurrentTransaction == null)
    {
      _transaction = await _context.Database.BeginTransactionAsync();
    }
  }

  public async Task CommitTransactionAsync()
  {
    if (_transaction != null)
    {
      try
      {
        await _context.SaveChangesAsync();
        await _transaction.CommitAsync();
      }
      catch
      {
        await RollbackTransactionAsync();
        throw;
      }
      finally
      {
        await DisposeTransactionAsync();
      }
    }
  }

  public async Task RollbackTransactionAsync()
  {
    if (_context.Database.CurrentTransaction != null)
    {
      await _context.Database.RollbackTransactionAsync();
    }
  }

  private async Task DisposeTransactionAsync()
  {
    if (_transaction != null)
    {
      await _transaction.DisposeAsync();
      _transaction = null!;
    }
  }

  public async ValueTask DisposeAsync()
  {
    await DisposeTransactionAsync();
    await _context.DisposeAsync();
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await _context.SaveChangesAsync();
  }
}