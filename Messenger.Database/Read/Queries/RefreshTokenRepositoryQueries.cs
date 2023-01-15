﻿namespace Messenger.Database.Read.Queries;

internal static class RefreshTokenRepositoryQueries
{
    internal const string GetRefreshToken = "SELECT * FROM public.refresh_token WHERE token = @token";
    internal const string GetRefreshTokenById = "SELECT * FROM public.refresh_token WHERE id = @id";
}