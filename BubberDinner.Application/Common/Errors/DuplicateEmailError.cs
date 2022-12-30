using System.Net;

namespace BubberDinner.Application.Common.Errors;

public record DuplicateEmailError : IError
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string ErrorMessage => "Email already exists";
}
