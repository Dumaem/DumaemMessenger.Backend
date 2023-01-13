using Messenger.Domain.Services;
using Messenger.Domain.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}