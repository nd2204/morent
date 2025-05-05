using System.Text.Json;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Morent.Application.Interfaces;
using Morent.Infrastructure.Settings;

namespace Morent.Infrastructure.Services;

public class OAuthService : IOAuthService
{
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly AppSettings _appSettings;

  public OAuthService(
      IHttpClientFactory httpClientFactory,
      IOptions<AppSettings> appSettings)
  {
    _httpClientFactory = httpClientFactory;
    _appSettings = appSettings.Value;
  }

  public async Task<OAuthLoginInfo?> ValidateExternalToken(string provider, string idToken)
  {
    return provider.ToLowerInvariant() switch
    {
      "google" => await ValidateGoogleToken(idToken),
      "github" => await ValidateGithubToken(idToken),
      _ => throw new ArgumentException($"Provider {provider} is not supported")
    };
  }

  private async Task<OAuthLoginInfo?> ValidateGoogleToken(string idToken)
  {
    // Create Google client
    var client = _httpClientFactory.CreateClient();

    // Validate token with Google
    var response = await client.GetAsync(
        $"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}");

    if (!response.IsSuccessStatusCode)
      return null;

    var content = await response.Content.ReadAsStringAsync();
    var payload = JsonSerializer.Deserialize<JsonElement>(content);

    // Verify the audience matches our client ID
    if (payload.GetProperty("aud").GetString() != _appSettings.GoogleClientId)
      return null;

    // TODO: handle invalid response
    var email = payload.GetProperty("email").GetString();
    Guard.Against.Null(email);
    var firstName = payload.GetProperty("given_name").GetString();
    Guard.Against.Null(firstName);
    var lastName = payload.GetProperty("family_name").GetString();
    Guard.Against.Null(lastName);
    var providerKey = payload.GetProperty("sub").GetString();
    Guard.Against.Null(providerKey);

    return new OAuthLoginInfo
    {
      Email = email,
      FirstName = firstName,
      LastName = lastName,
      ProviderKey = providerKey
    };
  }

  private async Task<OAuthLoginInfo?> ValidateGithubToken(string accessToken)
  {
    // Create GitHub client
    var client = _httpClientFactory.CreateClient();
    client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");
    client.DefaultRequestHeaders.Add("User-Agent", "CarRentalApp");

    // Get user info
    var response = await client.GetAsync("https://api.github.com/user");
    if (!response.IsSuccessStatusCode)
      return null;

    var content = await response.Content.ReadAsStringAsync();
    var user = JsonSerializer.Deserialize<JsonElement>(content);

    // GitHub may not provide email directly, so get emails separately
    var emailResponse = await client.GetAsync("https://api.github.com/user/emails");
    var emailContent = await emailResponse.Content.ReadAsStringAsync();
    var emails = JsonSerializer.Deserialize<JsonElement>(emailContent);

    // Find primary email
    string email = null!;
    foreach (var emailObj in emails.EnumerateArray())
    {
      if (emailObj.TryGetProperty("primary", out var primary) && primary.GetBoolean())
      {
        email = emailObj.GetProperty("email").GetString()!;
        break;
      }
    }

    // Parse name into first/last
    string fullName = user.GetProperty("name").GetString()!;
    string[] nameParts = (fullName ?? "").Split(' ');
    string firstName = nameParts.Length > 0 ? nameParts[0] : "";
    string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

    return new OAuthLoginInfo
    {
      Email = email,
      FirstName = firstName,
      LastName = lastName,
      ProviderKey = user.GetProperty("id").GetString()!
    };
  }
}
