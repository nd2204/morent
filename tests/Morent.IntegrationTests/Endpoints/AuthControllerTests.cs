using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Morent.WebApi;
using NimblePros.SampleToDo.FunctionalTests;
using Shouldly;
using Xunit;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Morent.IntegrationTests
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Register_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var registerRequest = new
            {
                Name = "Test User",
                Username = "testuser" + Guid.NewGuid().ToString("N").Substring(0, 6),
                Email = $"testuser_{Guid.NewGuid().ToString("N").Substring(0, 6)}@example.com",
                Password = "SecurePassword123!"
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(registerRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/register", content);
            
            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            authResponse.ShouldNotBeNull();
            authResponse.AccessToken.ShouldNotBeNullOrEmpty();
            // Verify cookie was set
            response.Headers.ShouldContain(h => h.Key == "Set-Cookie" && h.Value.Any(v => v.Contains("refreshToken")));
        }

        [Fact]
        public async Task Register_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var registerRequest = new
            {
                Name = "Test User",
                Username = "testuser",
                Email = "invalid-email",  // Invalid email format
                Password = "short"        // Too short password
            };
            
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(registerRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/register", content);
            
            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.ShouldContain("email");  // Should contain validation error about email
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnSuccess()
        {
            // Arrange
            // First register a user
            var username = "loginuser" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var email = $"login_{Guid.NewGuid().ToString("N").Substring(0, 6)}@example.com";
            var password = "SecurePassword123!";
            
            await RegisterTestUser(username, email, password);
            
            // Prepare login request
            var loginRequest = new
            {
                LoginId = email,  // Can be username or email based on your implementation
                Password = password
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", content);
            
            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            authResponse.ShouldNotBeNull();
            authResponse.AccessToken.ShouldNotBeNullOrEmpty();
            response.Headers.ShouldContain(h => h.Key == "Set-Cookie" && h.Value.Any(v => v.Contains("refreshToken")));
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnBadRequest()
        {
            // Arrange
            var loginRequest = new
            {
                LoginId = "nonexistent@example.com",
                Password = "WrongPassword123!"
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", content);
            
            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ThenLogin_ShouldWorkSuccessfully()
        {
            // Arrange - Register a new user
            var username = "testuser" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var email = $"testuser_{Guid.NewGuid().ToString("N").Substring(0, 6)}@example.com";
            var password = "SecurePassword123!";
            
            await RegisterTestUser(username, email, password);
            
            // Act - Login with the registered user
            var loginRequest = new
            {
                LoginId = email,
                Password = password
            };
            
            var loginContent = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json");
                
            var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);
            
            // Assert
            loginResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            
            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            authResponse.ShouldNotBeNull();
            authResponse.AccessToken.ShouldNotBeNullOrEmpty();
        }

        private async Task RegisterTestUser(string username, string email, string password)
        {
            var registerRequest = new
            {
                Name = "Test User",
                Username = username,
                Email = email,
                Password = password
            };
            
            var content = new StringContent(
                JsonSerializer.Serialize(registerRequest),
                Encoding.UTF8,
                "application/json");
                
            await _client.PostAsync("/api/auth/register", content);
        }
    }

    // DTO classes matching your API responses
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}