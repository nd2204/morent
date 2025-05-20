using Morent.Core.MediaAggregate;
using Morent.Core.MorentPaymentAggregate;
using Morent.Core.ValueObjects;
using Morent.Infrastructure.Data.Configs;

namespace Morent.Infrastructure.Data;

public class MorentDbContext(
  DbContextOptions<MorentDbContext> options,
  IDomainEventDispatcher? dispatcher) : DbContext(options)
{
  private readonly IDomainEventDispatcher? _dispatcher = dispatcher;

  public DbSet<MorentUser> Users => Set<MorentUser>();
  public DbSet<MorentCar> Cars => Set<MorentCar>();
  public DbSet<MorentCarImage> CarImages => Set<MorentCarImage>();
  public DbSet<MorentCarModel> CarModels => Set<MorentCarModel>();
  public DbSet<MorentRental> Rentals => Set<MorentRental>();
  public DbSet<MorentReview> Reviews => Set<MorentReview>();
  public DbSet<MorentImage> Images => Set<MorentImage>();
  public DbSet<MorentPayment> Payments => Set<MorentPayment>();
  public DbSet<PaymentProvider> PaymentMethods => Set<PaymentProvider>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MorentDbContext).Assembly);
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<HasDomainEventsBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }
}