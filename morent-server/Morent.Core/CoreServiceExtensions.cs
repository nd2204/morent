using System;
using Microsoft.Extensions.DependencyInjection;

namespace Morent.Core;

public static class CoreServiceExtensions
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
  {
    logger.LogInformation("{Project} services registered", "Core");

    return services;
  }
}
