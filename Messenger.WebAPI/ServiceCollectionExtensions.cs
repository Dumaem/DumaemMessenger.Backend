using System.Text;
using Messenger.WebAPI.Database.Repositories;
using Messenger.WebAPI.Database.Repositories.Impl;
using Messenger.WebAPI.Domain.Services;
using Messenger.WebAPI.Domain.Services.Impl;
using Messenger.WebAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.WebAPI;

public static class ServiceCollectionExtensions
{
    public static void RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();
    }

    public static void RegisterDatabaseRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
    
    public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(jwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });
    }
}