using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
      .AddInfrastructureServices(logger, builder.Configuration, builder.Environment.EnvironmentName)
      .AddCoreServices(logger)
      .AddMediatrConfigs()
      .AddGoogleConfigs(builder.Configuration)
      .AddJwtConfigs(builder.Configuration)
      .AddAuthorization()
      ;

    if (builder.Environment.IsDevelopment()) {
      // Use test mailing service in development
      // services.AddScoped<IEmailSender, Mime>;

      try {
        var ngrok = new Process
        {
          StartInfo = new ProcessStartInfo
          {
            FileName = "ngrok",
            Arguments = "http --url=coral-unbiased-scarcely.ngrok-free.app https://localhost:7083",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = false
          }
        };
        ngrok.Start();
      } catch(Exception ex) {
        logger.LogWarning("Errors when starting ngrok: {0}", ex.Message);
      }
    }

    logger.LogInformation("{Project} registered", "WebApi services");

    return services;
  }
}
