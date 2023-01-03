using BubberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using BubberDinner.Domain.Common.Errors;
using BubberDinner.Application.Services.Authentication.Common;
using BubberDinner.Application.Commands.Register;
using MediatR;
using BubberDinner.Application.Queries;

namespace BubberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        ErrorOr<AuthenticationResult> registerResult = await _mediator.Send(command);

        if(registerResult.IsError && registerResult.Errors.Any(x => x == Errors.Authentication.InvalidCredentials))
        {
            var description = registerResult
                .Errors
                .Find(x => x == Errors.Authentication.InvalidCredentials)
                .Description;

            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: description);
        }

        return registerResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);

        var loginResult = await _mediator.Send(query);

        return loginResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    #region Local

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);
    }

    #endregion
}