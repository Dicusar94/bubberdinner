using BubberDinner.Application.Common.Errors;
using BubberDinner.Application.Common.Interfaces.Authentication;
using BubberDinner.Application.Common.Interfaces.Persistence;
using BubberDinner.Domain.Entities;
using OneOf;

namespace BubberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Login(string email, string password)
    {
        if(_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("User with given email does not exist");
        }

        if (user.Password != password)
        {
            throw new Exception("Invalid password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }

    public OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password)
    {
        if(_userRepository.GetUserByEmail(email) is not null)
        {
            return new DuplicateEmailError();
        }

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}