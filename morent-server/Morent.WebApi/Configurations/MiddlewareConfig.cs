using System;
using Scalar.AspNetCore;

namespace Morent.WebApi.Configurations;

public static class MiddlewareConfig 
{
  public static IApplicationBuilder
  UseMiddleware(this WebApplication app)
  {
    app.UseAuthentication();

    if (app.Environment.IsDevelopment())
    {
      app.MapScalarApiReference();
    }
    else
    {
      app.UseHsts();
      app.UseHttpsRedirection();
    }

    app.UseAuthorization();
    app.UseCors("AllowReactApp");

    return app;
  }
}
