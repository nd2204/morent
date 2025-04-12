using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using  Microsoft.OpenApi.Models;
using Morent.Infrastructure;

namespace Morent.WebApi.Configurations;

public static class ServiceConfig
{
  public static IServiceCollection AddServiceConfigs(
    this IServiceCollection services,
    ILogger logger,
    WebApplicationBuilder builder
  )
  {
    logger.LogInformation("Registering WebApi services");

    services
      .AddInfrastructureServices(logger, builder.Configuration)
      .AddJwtConfigs(builder.Configuration)
      .AddCors(options =>
      {
        options.AddPolicy("AllowReactApp",
          builder => builder.WithOrigins("http://localhost:3000") // React app URL
            .AllowAnyMethod()
            .AllowAnyHeader());
      });

    logger.LogInformation("{Project} services registered", "WebApi");

    return services;
  }
}
