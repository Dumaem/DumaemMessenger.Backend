namespace Messenger.Database.Read.Queries;

internal static class UserRepositoryQueries
{
    internal const string GetUserByEmail = "SELECT * FROM public.user WHERE email = @email";
    internal const string GetUserById = "SELECT * FROM public.user WHERE email = @id";
    internal const string GetUserEncryptedPassword = "SELECT password FROM public.user WHERE id = @id";
}