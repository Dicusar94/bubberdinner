using BubberDinner.Domain.Entities;

namespace BubberDinner.Application.Services.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);