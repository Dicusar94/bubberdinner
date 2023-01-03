using BubberDinner.Application.Services.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BubberDinner.Application.Queries;

public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;