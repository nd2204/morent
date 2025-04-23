using System;
using Scalar.AspNetCore;

namespace Morent.WebApi.Configurations;

public static class MiddlewareConfig 
{
  public static IApplicationBuilder UseMiddleware(this WebApplication app)
  {
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
      app.MapOpenApi();
      app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();
    app.UseHsts();
    app.MapControllers();
    app.UseCors("All");

    app.UseAuthentication();
    app.UseAuthorization();

    app.Logger.LogInformation("{Project} registered", "WebApi Middlewares");

    return app;
  }
}
