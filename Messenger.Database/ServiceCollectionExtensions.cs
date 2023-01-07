using Messenger.Database.Repositories;
using Messenger.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Database;

public static class ServiceCollectionExtensions
{
    public static void RegisterDatabaseRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}