using Ardalis.SharedKernel;
using Morent.Application.Interfaces;
using Morent.Core.MediaAggregate;
using Morent.Infrastructure.Data;
using Scalar.AspNetCore;

namespace Morent.WebApi.Configurations;

public static class MiddlewareConfig 
{
  public static async Task<IApplicationBuilder> UseMiddleware(this WebApplication app)
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
      app.Use(async (context, next) =>
      {
        Console.WriteLine("PATH: " + context.Request.Path);
        Console.WriteLine("QUERY: " + context.Request.QueryString);
        Console.WriteLine("COOKIES:");
        foreach (var cookie in context.Request.Cookies)
        {
          Console.WriteLine($"{cookie.Key}: {cookie.Value}");
        }
        await next();
      });
      app.MapOpenApi();
      app.MapScalarApiReference();
    }
    app.UseMiddleware<AuthorizationMiddleware>();
    // app.UseMiddleware<ImageValidationMiddleware>(new ImageValidationOptions());

    app.UseStaticFiles(); // Enable serving static files from wwwroot
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseHsts();
    app.MapControllers();
    app.UseCors("All");

    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    app.Logger.LogInformation("{Project} registered", "WebApi Middlewares");

    await SeedDatabase(app);

    return app;
  }

  static async Task SeedDatabase(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
      var context = services.GetRequiredService<MorentDbContext>();
      //          context.Database.Migrate();
      context.Database.EnsureCreated();
      await new SeedData(
        context,
        services.GetRequiredService<IAuthService>(),
        services.GetRequiredService<IImageService>(),
        services.GetRequiredService<IImageStorage>(),
        services.GetRequiredService<IWebHostEnvironment>(),
        services.GetRequiredService<IRepository<MorentImage>>()
      ).InitializeAsync();
    }
    catch (Exception ex)
    {
      var logger = services.GetRequiredService<ILogger<Program>>();
      logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
  }
}
