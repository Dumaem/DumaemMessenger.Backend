﻿using Messenger.Domain.Exception;
using Messenger.WebAPI.Shared.SignalR;
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
            if (ex.ExceptionType != ChatExceptionType.ExpiredToken) return ex;
            await invocationContext.Hub.Clients.Caller.SendAsync(SignalRClientMethods.Unauthorized, invocationContext.HubMethodName, invocationContext.HubMethodArguments);
            invocationContext.Context.Abort();
            return ex;
        
        }
        catch (Exception ex)
        {
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