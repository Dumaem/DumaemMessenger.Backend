namespace Messenger.Database.Read.Queries;

internal static class RefreshTokenRepositoryQueries
{
    internal const string GetRefreshToken = "SELECT * FROM public.refresh_token WHERE token = @token";
    
    internal const string GetActualTokenByUserAndDeviceId = 
        "SELECT * FROM public.refresh_token WHERE user_id = @userId and device_id = @deviceId " +
        "and not is_revoked and not is_used";
    
    internal const string UpdateRefreshTokenUse = "UPDATE public.refresh_token SET is_used = true WHERE id = @id";
    
    internal const string UpdateRefreshTokenRevoke =
        "UPDATE public.refresh_token SET is_revoked = true WHERE id = @id";
}