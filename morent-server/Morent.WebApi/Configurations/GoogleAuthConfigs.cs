using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace Morent.WebApi.Configurations;

public static class GoogleAuthConfigs
{
  public static IServiceCollection AddGoogleConfigs(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddAuthentication(options => {
      options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
      options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
      options.LoginPath = "/api/auth/login";
      options.LogoutPath = "/api/auth/logout";
      // Set cookie properties to ensure state is preserved
      options.Cookie.SameSite = SameSiteMode.Lax; // Important for OAuth redirects
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // For HTTPS
    })
    .AddGoogle(options =>
    {
      options.ClientId = configuration.GetValue<string>("AppSettings:GoogleClientId")!;
      options.ClientSecret = configuration.GetValue<string>("AppSettings:GoogleClientSecret")!;
      options.CallbackPath = "/api/auth/google-callback";
      options.CorrelationCookie.MaxAge = TimeSpan.FromMinutes(10);

      // Map Google claims to your application claims
      options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
      options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
      options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
      options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
      options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

      options.SaveTokens = true; // Save the token for later use

      //   // Optional: Custom callbacks to handle token events
      //   options.Events = new OAuthEvents
      //   {
      //     OnCreatingTicket = context =>
      //     {
      //       // Custom logic before creating the authentication ticket
      //       return Task.CompletedTask;
      //     },
      //     OnTicketReceived = context =>
      //     {
      //       // Custom logic after receiving the ticket
      //       return Task.CompletedTask;
      //     }
      //   };
      // }
    });
    return services;
  }
}