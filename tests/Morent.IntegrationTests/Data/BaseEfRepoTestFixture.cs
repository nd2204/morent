using System;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Morent.Core.MorentCarAggregate;
using Morent.Infrastructure.Data;
using Morent.Infrastructure.Data.Repositories;
using NSubstitute;

namespace Morent.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
  protected MorentDbContext _dbContext;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    var _fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();

    _dbContext = new MorentDbContext(options, _fakeEventDispatcher);
  }

  protected static DbContextOptions<MorentDbContext> CreateNewContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<MorentDbContext>();
    builder.UseInMemoryDatabase("morenttestdb")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected EFRepository<T> GetRepository<T>() where T : class, IAggregateRoot
  {
    return new EFRepository<T>(_dbContext);
  }

  protected UserRepository GetUserRepository() => new(_dbContext);
  protected CarRepository GetCarRepository() => new(_dbContext);
  protected ReviewRepository GetReviewRepository() => new(_dbContext);
  protected ReviewRepository GetRentalRepository() => new(_dbContext);
}