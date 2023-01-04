using BubberDinner.Application.Authentication.Commands.Register;
using BubberDinner.Application.Authentication.Common;
using BubberDinner.Application.Authentication.Queries.Login;
using BubberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using BubberDinner.Domain.Common.Errors;
using MediatR;
using MapsterMapper;

namespace BubberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);

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
        var query = _mapper.Map<LoginQuery>(request);

        var loginResult = await _mediator.Send(query);

        return loginResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    #region Local

    private AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return _mapper.Map<AuthenticationResponse>(authResult);
    }

    #endregion
}