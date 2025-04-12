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

    logger.LogInformation("{Project} services registered", "Infrastructure");
    return services;
  }
}
