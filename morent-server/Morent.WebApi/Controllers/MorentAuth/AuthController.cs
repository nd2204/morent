using Ardalis.Result;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var userDto = await mediator_.Send(new LoginUserCommand(request.UsernameOrEmail, request.Password));
        return new LoginResponse(new Guid(), "access token here", "refresh token here");
    }

    [HttpPost(SignupRequest.Route)]
    public async Task<ActionResult<SignupResponse>> Signup(SignupRequest request)
    {
        var command = new CreateUserCommand(request.Username, request.Password, request.Email);
        var result = await mediator_.Send(command);
        var value = result.Value;

        if (value != null)
        {
            return Ok(new SignupResponse { Id = value.Id, Role = value.Role, Name = value.Name});
        }

        return BadRequest(result.Errors);
    }
}