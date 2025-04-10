using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Morent.Infra.Data.Persistence;
using Morent.Core.Interfaces;

namespace Morent.Infra.Services;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    if (env == "Development")
    {
      services.AddDbContext<MorentDbContext>(options =>
          options.UseSqlite(
              configuration.GetConnectionString("DefaultConnection"),
              b => b.MigrationsAssembly("Morent.Infra")));
    }
    else
    {
      services.AddDbContext<MorentDbContext>(options =>
          options.UseSqlServer(
              configuration.GetConnectionString("DefaultConnection"),
              b => b.MigrationsAssembly("Morent.Infra")));
    }

    services.AddScoped<IMorentDbContext>(provider =>
        provider.GetRequiredService<MorentDbContext>());

    return services;
  }
}