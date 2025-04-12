using Morent.Application.Features.CarModels.Create;
using Morent.Core.Entities;
using System.Reflection;

namespace Morent.WebApi.Configurations;

public static class MediatrConfigs
{
  public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
  {
    var mediatRAssemblies = new[]
      {
        Assembly.GetAssembly(typeof(MorentCarModel)), // Core
        Assembly.GetAssembly(typeof(CreateCarModelCommand)) // Application
      };

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));

    return services;
  }
}

