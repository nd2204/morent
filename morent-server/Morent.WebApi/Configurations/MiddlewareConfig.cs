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
      app.Use(async (context, next) =>
      {
        if (context.Request.Path == "/")
        {
          context.Response.Redirect("/scalar");
          return;
        }
        await next();
      });
      app.MapScalarApiReference();
    }

    app.UseAuthorization();
    app.UseCors("AllowReactApp");

    return app;
  }
}
