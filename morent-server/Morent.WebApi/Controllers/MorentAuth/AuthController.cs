using Ardalis.Result;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features;
using Morent.Application.Features.Users.Commands.Create;
using Morent.Application.Features.Users.Commands.Login;
using Morent.WebApi.Controllers.MorentAuth.Login;
using Morent.WebApi.Controllers.MorentAuth.Signup;

namespace Morent.Api.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger_;
    private readonly IMediator mediator_;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        mediator_ = mediator;
        logger_ = logger;
    }

    [HttpPost(LoginRequest.Route)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await mediator_.Send(new LoginUserCommand(request.UsernameOrEmail, request.Password));

        if (result.IsError()) {
            var error =  result.Errors.First() ?? ""; 
            return NotFound(new ErrorResponse { ErrorMessage = error });
        }

        var value = result.Value;
        Request.Headers.Cookie.Append($"refreshToken={"REFRESH_TOKEN"}");

        return Ok(new LoginResponse {
            UserId = value.Id,
            Email = value.Email,
            AccessToken = value.AccessToken,
            RefreshToken = value.RefreshToken,
            ExpiresIn = value.ExpiresIn
        });
    }

    [HttpPost(SignupRequest.Route)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SignupResponse>> Signup(SignupRequest request)
    {
        var command = new CreateUserCommand(request.Username, request.Password, request.Email);
        var result = await mediator_.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new ErrorResponse { ErrorMessage = "I will add better error handling" });
        }

        var value = result.Value;
        return CreatedAtAction(nameof(Signup),new SignupResponse {
            UserId = value.Id,
            Email = value.Email,
            AccessToken = value.AccessToken,
            RefreshToken = value.RefreshToken,
            ExpiresIn = value.ExpiresIn
        });
    }
}