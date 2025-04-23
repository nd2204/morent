using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.DTOs;
using Morent.Application.Features;
using Morent.Application.Features.Auth;

namespace Morent.Api.Controllers;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var command = new LoginUserQuery(request.LoginId, request.Password);
            var response = await _mediator.Send(command);

            // Set refresh token in HttpOnly cookie
            SetRefreshTokenCookie(response.RefreshToken);

            // Remove refresh token from response body for security
            // but keep a copy of it in the response for mobile clients
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Signup([FromBody] RegisterUserCommand request)
    {
        try
        {
            var command = new RegisterUserCommand(
                request.Name,
                request.Username,
                request.Email,
                request.Password);

            var response = await _mediator.Send(command);

            // Set refresh token in HttpOnly cookie
            SetRefreshTokenCookie(response.RefreshToken);

            return Ok(response);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var response = await _mediator.Send(command);

            // For enhanced security, set refresh token in an HttpOnly cookie
            // And return only the access token in the response body
            SetRefreshTokenCookie(response.RefreshToken);

            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid refresh token" });
        }
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

    [HttpPost("oauth")]
    public async Task<ActionResult<AuthResponse>> ExternalLogin([FromBody] OAuthLoginRequest request)
    {
        try
        {
            var command = new OAuthLoginCommand(request.Provider, request.ProviderToken);

            var response = await _mediator.Send(command);

            // Set refresh token in HttpOnly cookie
            SetRefreshTokenCookie(response.RefreshToken);

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // Get user ID from claims
        var userId = User.FindFirst("uid")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Get refresh token from cookie
        var refreshToken = Request.Cookies["refreshToken"];

        // Revoke the token
        if (refreshToken != null)
        {
            var command = new RevokeTokenCommand(refreshToken, userId);
            await _mediator.Send(command);
        }

        // Clear the cookie
        Response.Cookies.Delete("refreshToken");

        return Ok(new { message = "Logged out successfully" });
    }
}