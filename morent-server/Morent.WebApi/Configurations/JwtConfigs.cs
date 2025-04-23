using System;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Morent.Infrastructure.Settings;

namespace Morent.WebApi.Configurations;

public static class JwtConfigs
{
  public static IServiceCollection AddJwtConfigs(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration.GetValue<string>("AppSettings:JwtIssuer"),
        ValidAudience = configuration.GetValue<string>("AppSettings:JwtAudience"),
        IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:JwtSecret")!))
      };
    })
    ;

    // Update AppSettings to include OAuth client IDs
    services.Configure<AppSettings>(options =>
    {
      configuration.GetSection("AppSettings").Bind(options);
    });

    return services;
  }
}
