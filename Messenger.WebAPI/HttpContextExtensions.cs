namespace Messenger.WebAPI;

public static class HttpContextExtensions
{
    /// <summary>
    /// Get an user id from request
    /// </summary>
    /// <returns>Sender's user id</returns>
    public static string GetUserId(this HttpContext httpContext)
    {
        return httpContext.User.Claims.Single(x => x.Type == "id").Value;
    }
}