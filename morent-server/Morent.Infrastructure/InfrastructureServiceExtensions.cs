using Morent.Infrastructure.Data;
using Morent.Infrastructure.Data.Repositories;
using Morent.Application.Repositories;
using Morent.Infrastructure.Settings;
using Morent.Application.Interfaces;
using Morent.Infrastructure.Services;

namespace Morent.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ILogger logger,
        ConfigurationManager configuration
    )
    {
        services.AddMorentDbContext(configuration);
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        services.AddHttpClient();

        services
            .AddScoped(typeof(IRepository<>), typeof(EFRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EFRepository<>))
            .AddScoped<ICarRepository, CarRepository>()
            .AddScoped<IRentalRepository, RentalRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IOAuthService, OAuthService>()
            .AddScoped<IAuthService, AuthService>();

        logger.LogInformation("{Project} registered", "Infrastructure services");
        return services;
    }
}
