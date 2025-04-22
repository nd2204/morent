using Ardalis.GuardClauses;
using Morent.Application.Interfaces;
using Morent.Application.Services;
using Morent.Core.Interfaces;
using Morent.Infrastructure.Data;
using Morent.Infrastructure.Services;

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
      .AddScoped<IAuthService, AuthService>()
      .AddScoped<IUserService, UserService>()
    ;

    logger.LogInformation("{Project} registered", "Infrastructure services");
    return services;
  }
}
