using System;

namespace Morent.Infrastructure.Data;

public static class MorentDbContextExtensions
{
  public static void AddMorentDbContext(
    this IServiceCollection services,
    ConfigurationManager configuration
  )
  {
    string connectionString = configuration.GetConnectionString("SqliteConnection")!;
    services.AddDbContext<MorentDbContext>(options =>
      options.UseSqlite(connectionString));
  }
}
