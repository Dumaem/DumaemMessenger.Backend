namespace Messenger.Database.Read.Queries;

internal static class UserVerificationRepositoryQueries
{
    internal const string GetVerifyToken =
        "SELECT * FROM public.verification WHERE user_id = @userId and is_actual";

    internal const string CreateToken = @"INSERT INTO public.verification (token, expiry_date, user_id) 
            VALUES (@token, @expiryDate, @userId)";
}