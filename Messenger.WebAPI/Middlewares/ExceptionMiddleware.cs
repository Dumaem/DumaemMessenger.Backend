using FluentValidation;
using Messenger.Domain.Exception;
# if !DEBUG
using Messenger.Domain.ErrorMessages;
#endif

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
        catch (ValidationException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                e.Errors
            });
        }
        catch (Exception e)
        {
            # if DEBUG
            var errorMessage = e;
            # else
            var errorMessage = ServerErrorMessages.InternalServerError;
            #endif

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            _logger.Log(LogLevel.Error, "{ErrorMessage}", e.ToString());
            await context.Response.WriteAsJsonAsync(new
            {
                Message = errorMessage
            });
        }
    }
}