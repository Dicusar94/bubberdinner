using BubberDinner.Application.Common.Interfaces.Authentication;
using BubberDinner.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace BubberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return service;
    }
}