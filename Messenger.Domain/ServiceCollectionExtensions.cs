using FluentValidation;
using FluentValidation.AspNetCore;
using Messenger.Domain.Services;
using Messenger.Domain.Services.Impl;
using Messenger.Domain.Validation.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IChatService, ChatService>();

        return services;
    }

    public static IServiceCollection AddDomainValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(UserValidator).Assembly);

        return services;
    }
}