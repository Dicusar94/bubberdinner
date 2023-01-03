using BubberDinner.Application.Services.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BubberDinner.Application.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;