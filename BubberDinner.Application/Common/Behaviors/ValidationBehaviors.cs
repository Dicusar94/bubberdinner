using BubberDinner.Application.Authentication.Commands.Register;
using BubberDinner.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BubberDinner.Application.Common.Behaviors;

public class ValidationRegisterCommandBehavior : IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand request,
        RequestHandlerDelegate<ErrorOr<AuthenticationResult>> next,
        CancellationToken cancellationToken)
    {
        // before the handler
        var result = await next();
        // after the handler
        return result;
    }
}