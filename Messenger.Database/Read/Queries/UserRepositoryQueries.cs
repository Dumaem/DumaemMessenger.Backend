namespace Messenger.Database.Read.Queries;

internal static class UserRepositoryQueries
{
    internal const string GetUserByEmailQuery = "SELECT * FROM public.user WHERE email = @email";
}