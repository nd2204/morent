using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using  Microsoft.OpenApi.Models;
using Morent.Core;
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
    services
      .AddInfrastructureServices(logger, builder.Configuration)
      .AddCoreServices(logger)
      .AddMediatrConfigs()
      .AddJwtConfigs(builder.Configuration)
      .AddCors(options =>
      {
        options.AddPolicy("AllowReactApp",
          builder => builder.WithOrigins("http://localhost:3000") // React app URL
            .AllowAnyMethod()
            .AllowAnyHeader());
      })
      .AddCors(options =>
      {
        options.AddPolicy("All", builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
      })
      .AddAuthorization()
      ;

    logger.LogInformation("{Project} registered", "WebApi services");

    return services;
  }
}
