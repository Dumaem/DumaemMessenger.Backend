namespace Messenger.Database.Read.Queries;

internal static class UserVerificationRepositoryQueries
{
    internal const string GetVerifyToken =
        "SELECT token, expiry_date FROM public.verification WHERE user_id = @userId and is_actual";

    internal const string CreateToken = @"INSERT INTO public.verification (token, expiry_date, user_id) 
            VALUES (@token, @expiryDate, @userId)";

    internal const string VerifyUser = @"UPDATE public.user SET is_verified = true WHERE id = @id";
    
    internal const string RevokeExpiredToken = @"UPDATE public.verification SET is_actual = false WHERE token = @token";

    internal const string GetUserByToken = @"SELECT u.* FROM public.verification v JOIN public.user u
                                                ON v.user_id = u.id WHERE token = @token";
}