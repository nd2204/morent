using System;
using Microsoft.Extensions.DependencyInjection;
using Morent.Core.Interfaces;
using Morent.Core.Services;

namespace Morent.Core;

public static class CoreServiceExtensions
{
  public static IServiceCollection AddCoreServices(this IServiceCollection services, ILogger logger)
  {
    // TODO: add core services here
    services.AddScoped<ISecurityService, SecurityService>();
    services.AddScoped<IUserService, UserService>();

    logger.LogInformation("{Project} services registered", "Core");

    return services;
  }
}
