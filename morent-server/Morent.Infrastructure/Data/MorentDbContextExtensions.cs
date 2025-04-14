using System;
using Ardalis.GuardClauses;

namespace Morent.Infrastructure.Data;

public static class MorentDbContextExtensions
{
  public static void AddMorentDbContext(
    this IServiceCollection services,
    ConfigurationManager configuration
  )
  {
    string? connectionString = configuration.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);

    services.AddDbContext<MorentDbContext>(options =>
      options.UseSqlite(connectionString));
  }
}
