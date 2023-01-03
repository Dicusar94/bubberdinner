using BubberDinner.Application.Services.Authentication.Common;
using ErrorOr;

namespace BubberDinner.Application.Services.Authentication.Queries;

public interface IAuthenticationQueryService
{
    ErrorOr<AuthenticationResult> Login(string email, string password);
}