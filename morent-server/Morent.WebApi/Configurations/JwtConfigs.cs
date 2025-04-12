using System;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
    // .AddJwtBearer(options =>
    // {
    //   options.TokenValidationParameters = new TokenValidationParameters
    //   {
    //     ValidateIssuer = true,
    //     ValidateAudience = true,
    //     ValidateLifetime = true,
    //     ValidateIssuerSigningKey = true,
    //     IssuerSigningKey = new SymmetricSecurityKey(
    //       Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret")))
    //   };
    // })
    ;

    services.AddAuthorization();

    return services;
  }
}
