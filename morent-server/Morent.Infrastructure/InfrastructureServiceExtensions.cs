using Morent.Infrastructure.Data;
using Morent.Infrastructure.Data.Repositories;
using Morent.Application.Repositories;
using Morent.Infrastructure.Settings;
using Morent.Infrastructure.Services;
using Ardalis.GuardClauses;
using Stripe;
using Morent.Infrastructure.Payment;

namespace Morent.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ILogger logger,
        ConfigurationManager configuration,
        string environmentName
    )
    {
        if (environmentName == "Development")
        {
            RegisterDevelopmentOnlyDependencies(services, configuration);
        }
        else if (environmentName == "Testing")
        {
            RegisterTestingOnlyDependencies(services);
        }
        else
        {
            RegisterProductionOnlyDependencies(services, configuration);
        }

        RegisterEFRepositories(services);

        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

        services.AddHttpClient();

        logger.LogInformation("{Project} registered", "Infrastructure services");
        return services;
    }

    private static void AddDbContextWithSqlite(IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("SqliteConnection");
        Guard.Against.Null(connectionString);
        services.AddDbContext<MorentDbContext>(options =>
          options.UseSqlite(connectionString));
    }

    private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services, IConfiguration configuration)
    {
        AddDbContextWithSqlite(services, configuration);
        // services.AddScoped<IEmailSender, SmtpEmailSender>();
        // services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
        // services.AddScoped<IListIncompleteItemsQueryService, ListIncompleteItemsQueryService>();
        // services.AddScoped<IListProjectsShallowQueryService, ListProjectsShallowQueryService>();
    }

    private static void RegisterTestingOnlyDependencies(IServiceCollection services)
    {
        // do not configure a DbContext here for testing - it's configured in CustomWebApplicationFactory

        // services.AddScoped<IEmailSender, FakeEmailSender>();
        // services.AddScoped<IListContributorsQueryService, FakeListContributorsQueryService>();
        // services.AddScoped<IListIncompleteItemsQueryService, FakeListIncompleteItemsQueryService>();
        // services.AddScoped<IListProjectsShallowQueryService, FakeListProjectsShallowQueryService>();
    }

    private static void RegisterProductionOnlyDependencies(IServiceCollection services, IConfiguration configuration)
    {
        AddDbContextWithSqlite(services, configuration);

        // services.AddScoped<IEmailSender, SmtpEmailSender>();
        // services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
        // services.AddScoped<IListIncompleteItemsQueryService, ListIncompleteItemsQueryService>();
        // services.AddScoped<IListProjectsShallowQueryService, ListProjectsShallowQueryService>();
    }

    private static void RegisterEFRepositories(IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepository<>), typeof(EFRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EFRepository<>))

            .AddSingleton<IImageStorage, LocalFileSystemStorage>()
            .AddScoped<IUnitOfWork, EfUnitOfWork>()

            .AddScoped<ICarRepository, CarRepository>()
            .AddScoped<IRentalRepository, RentalRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IUserRepository, UserRepository>()

            .AddScoped<IOAuthService, OAuthService>()
            .AddScoped<IAuthService, AuthService>()

            .AddScoped<IImageService, ImageService>()
            .AddScoped<ICarImageService, CarImageService>()
            .AddScoped<IUserProfileService, UserService>()
            .AddScoped<IUserService, UserService>()

            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IPaymentProvider, StripePaymentProvider>()
            .AddScoped<IPaymentProvider, MoMoPaymentProvider>()
            .AddScoped<IPaymentProvider, VNPayPaymentMethod>()
            ;
    }
}
