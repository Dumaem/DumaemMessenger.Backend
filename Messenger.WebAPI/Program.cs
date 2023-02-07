using Messenger.Database;
using Messenger.Domain;
using Messenger.Migrator;
using Messenger.WebAPI;
using Messenger.WebAPI.Chat;
using Messenger.WebAPI.Middlewares;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR().AddHubOptions<ChatHub>(options =>
{
    options.AddFilter(typeof(ChatHubAuthorizationFilter));
}).AddJsonProtocol();

builder.Services
    .AddEndpointsApiExplorer()
    .ConfigureAuthorization(builder.Configuration)
    .RegisterDatabaseSources(builder.Configuration)
    .RegisterDomainServices()
    .AddDomainValidation()
    .RegisterDatabaseRepositories()
    .AddMigrations(builder.Configuration)
    .AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Get token from [POST] /login endpoint and paste it here with this template: Bearer {Your token}",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    });

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/z");

app.Run();