using System.Text.Json.Serialization;
using Morent.WebApi.Configurations;
using Serilog;
using Serilog.Extensions.Logging;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Preserve property names
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Preserve property names
            });


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi("v1");

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp",
                builder => builder.WithOrigins("http://localhost:3000") // React app URL
                           .AllowAnyMethod()
                           .AllowAnyHeader());
        });

        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        logger.Information("Starting Morent API...");

        builder.AddLoggerConfigs();

        var appLogger = new SerilogLoggerFactory(logger)
            .CreateLogger<Program>();

        builder.Services.AddServiceConfigs(appLogger, builder);

        var app = builder.Build();

        await app.UseMiddleware();

        app.Run();
    }
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}