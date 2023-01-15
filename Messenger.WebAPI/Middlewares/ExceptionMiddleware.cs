using Messenger.Domain.Exception;

namespace Messenger.WebAPI.Middlewares;

public class ExceptionMiddleware
{
    public readonly RequestDelegate Next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        Next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (MigrationException)
        {
            _logger.LogCritical("Migrations are failed. Contact developer for help");
            throw;
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            _logger.Log(LogLevel.Information, "{ErrorMessage}", e.ToString());
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Internal error. Contact developer to solve the problem."
            });
        }
    }
}