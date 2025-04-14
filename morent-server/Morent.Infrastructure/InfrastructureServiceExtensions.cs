using Ardalis.GuardClauses;
using Morent.Infrastructure.Data;

namespace Morent.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ILogger logger,
    ConfigurationManager configuration
  )
  {
    services.AddMorentDbContext(configuration);

    services
      .AddScoped(typeof(IRepository<>), typeof(EFRepository<>))
      .AddScoped(typeof(IReadRepository<>), typeof(EFRepository<>))
    ;

    logger.LogInformation("{Project} registered", "Infrastructure services");
    return services;
  }
}
