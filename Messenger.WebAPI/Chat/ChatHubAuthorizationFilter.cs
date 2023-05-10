using System.Net;
using Messenger.Domain.Exception;
using Messenger.WebAPI.Shared.Client;
using Messenger.WebAPI.Shared.SharedModels;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Chat;

public class ChatHubAuthorizationFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            if (IsTokenExpired(invocationContext))
                throw new ChatException(ChatExceptionType.ExpiredToken);

            return await next(invocationContext);
        }
        catch (ChatException ex)
        {
            await invocationContext.Hub.Clients.Caller.SendAsync(SignalRClientMethods.Unauthorized,
                new UnauthorizedAccessContext { Message = ex.Message, StatusCode = HttpStatusCode.Unauthorized});
            invocationContext.Context.Abort();
            return ex;
        }
        catch (Exception ex)
        {
            invocationContext.Context.Abort();
            return ex;
        }
    }

    private static bool IsTokenExpired(HubInvocationContext invocationContext)
    {
        var exp = invocationContext.Context.User!.Claims.First(x => x.Type == "exp").Value;

        var dateExp = DateTimeOffset.FromUnixTimeSeconds(int.Parse(exp));
        return DateTime.UtcNow > dateExp;
    }
}