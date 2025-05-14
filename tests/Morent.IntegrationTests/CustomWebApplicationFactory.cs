using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Morent.Infrastructure.Data;

namespace NimblePros.SampleToDo.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  /// <summary>
  /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
  /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Testing"); // will not send real emails
    var host = builder.Build();
    host.Start();

    // Get service provider.
    var serviceProvider = host.Services;

    // Create a scope to obtain a reference to the database
    // context (AppDbContext).
    using (var scope = serviceProvider.CreateScope())
    {
      var scopedServices = scope.ServiceProvider;
      var db = scopedServices.GetRequiredService<MorentDbContext>();

      var logger = scopedServices
          .GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

      // Ensure the database is created.
      db.Database.EnsureCreated();

      try
      {
        // Can also skip creating the items
        //if (!db.ToDoItems.Any())
        //{
        // Seed the database with test data.
        SeedData.InitializeAsync(serviceProvider, logger).GetAwaiter().GetResult();
        //}
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {exceptionMessage}", ex.Message);
      }
    }

    return host;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder
        .ConfigureServices(services =>
        {
          // Remove the app's ApplicationDbContext registration.
          var descriptors = services.Where(
            d => d.ServiceType == typeof(MorentDbContext) ||
                 d.ServiceType == typeof(DbContextOptions<MorentDbContext>))
                .ToList();

          foreach(var descriptor in descriptors)
          {
            services.Remove(descriptor);
          }

          // This should be set for each individual test run
          string inMemoryCollectionName = Guid.NewGuid().ToString();

          // Add ApplicationDbContext using an in-memory database for testing.
          services.AddDbContext<MorentDbContext>(options =>
          {
            options.UseInMemoryDatabase(inMemoryCollectionName);
          });

          // Add MediatR
          services.AddMediatR(cfg =>
          {
            cfg.RegisterServicesFromAssemblies(
              typeof(Program).Assembly,
              Morent.Application.AssemblyReference.Assembly, // WebApi
              typeof(MorentDbContext).Assembly); // Infrastructure
          });
        });
  }
}
