using System.Text.RegularExpressions;
using BubberDinner.Application.Common.Errors;
using BubberDinner.Application.Services.Authentication;
using BubberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace BubberDinner.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        OneOf<AuthenticationResult, IError> registerResult = _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        return registerResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            error => Problem(statusCode: (int)error.StatusCode, title: error.ErrorMessage)
        );
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
        authResult.User.Id,
        authResult.User.FirstName,
        authResult.User.LastName,
        authResult.User.Email,
        authResult.Token);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(request.Email, request.Password);

        var authResponse = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);

        return Ok(authResponse);
    }
}