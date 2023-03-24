using Messenger.Database.Read;
using Messenger.Database.Repositories;
using Messenger.Database.Write;
using Messenger.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDatabaseRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();

        return services;
    }

    public static IServiceCollection RegisterDatabaseSources(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddTransient<MessengerReadonlyContext>(_ => new MessengerReadonlyContext(connectionString));

        services.AddDbContext<MessengerContext>(options =>
        {
            options.UseNpgsql(connectionString,
                postgresOptions =>
                {
                    postgresOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                });
        });

        return services;
    }
}