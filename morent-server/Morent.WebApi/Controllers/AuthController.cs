using System.Security.Claims;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Morent.WebApi.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger_;
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        logger_ = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        Result.Invalid();
        var command = new LoginUserQuery(request.LoginId, request.Password);
        var result = await _mediator.Send(command);

        // Set refresh token in HttpOnly cookie
        if (result.IsSuccess)
        {
            SetRefreshTokenCookie(result.Value.RefreshToken);
        }

        // Remove refresh token from response body for security
        // but keep a copy of it in the response for mobile clients
        return this.ToActionResult(result);
    }

    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Signup([FromBody] RegisterUserCommand request)
    {
        var command = new RegisterUserCommand(
            request.Name,
            request.Username,
            request.Email,
            request.Password);

        var result = await _mediator.Send(command);

        // Set refresh token in HttpOnly cookie
        if (result.IsSuccess)
            SetRefreshTokenCookie(result.Value.RefreshToken);

        return this.ToActionResult(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await _mediator.Send(command);

        // For enhanced security, set refresh token in an HttpOnly cookie
        // And return only the access token in the response body
        if (result != null)
        {
            SetRefreshTokenCookie(result.Value.RefreshToken);
        }

        return result.ToActionResult(this);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // Get user ID from claims
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        logger_.LogWarning("{0}", User.FindFirst(ClaimTypes.NameIdentifier));

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        // Get refresh token from cookie
        var refreshToken = Request.Cookies["refreshToken"];

        // Revoke the token
        if (refreshToken != null)
        {
            var command = new RevokeTokenCommand(refreshToken, userGuid);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return this.ToActionResult(result);
        }

        // Clear the cookie
        Response.Cookies.Delete("refreshToken");

        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback), "auth", null, Request.Scheme),
            Items = { { "scheme", GoogleDefaults.AuthenticationScheme }
            }
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return BadRequest("Google authentication failed");

        // Extract user info from the Google claims
        var userEmail = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = authenticateResult.Principal.FindFirstValue(ClaimTypes.GivenName);
        var lastName = authenticateResult.Principal.FindFirstValue(ClaimTypes.Surname);
        var googleId = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get the access token provided by Google (for calls to Google APIs)
        var accessToken = authenticateResult.Properties.GetTokenValue("access_token");
        var idToken = authenticateResult.Properties.GetTokenValue("id_token")!;

        // Process the external login using your application's logic
        var command = new OAuthLoginCommand("Google", idToken);
        // Use your mediator to handle the command
        var authResponse = await _mediator.Send(command);

        // Set refresh token in cookie
        SetRefreshTokenCookie(authResponse.RefreshToken);

        // Sign in with cookie authentication
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            authenticateResult.Principal,
            authenticateResult.Properties);

        return Ok();
    }

    // Helper method to set refresh token in HttpOnly cookie
    private void SetRefreshTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Use in production with HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}