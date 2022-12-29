using BubberDinner.Application.Common.Interfaces.Authentication;
using BubberDinner.Application.Common.Interfaces.Services;
using BubberDinner.Infrastructure.Authentication;
using BubberDinner.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BubberDinner.Application.Common.Interfaces.Persistence;
using BubberDinner.Infrastructure.Persistence;

namespace BubberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, ConfigurationManager configuration)
    {
        service.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        service.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        service.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        service.AddScoped<IUserRepository, UserRepository>();

        return service;
    }
}