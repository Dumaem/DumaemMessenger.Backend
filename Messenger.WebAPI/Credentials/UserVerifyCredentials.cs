namespace Messenger.WebAPI.Credentials;

public class UserVerifyCredentials
{
    public string UserEmail { get; set; } = null!;
    public string VerifyToken { get; set; } = null!;
}