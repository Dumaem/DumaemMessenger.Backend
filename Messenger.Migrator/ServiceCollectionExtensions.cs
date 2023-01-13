using FluentMigrator.Runner;
using Messenger.Migrator.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Migrator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMigrations(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(r => r
                .AddPostgres()
                .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection")!)
                .ScanIn(typeof(InitialMigration).Assembly)
                .For.Migrations())
            .AddLogging(log => log.AddFluentMigratorConsole());
        return services;
    }
}