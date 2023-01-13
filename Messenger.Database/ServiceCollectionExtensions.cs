using Messenger.Database.Repositories;
using Messenger.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDatabaseRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
    
    public static IServiceCollection RegisterDatabaseSources(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<NgpsqlContext>(_ => new NgpsqlContext(configuration.GetConnectionString("DefaultConnection")!));

        return services;
    }
}