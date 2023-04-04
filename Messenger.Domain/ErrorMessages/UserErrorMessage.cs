namespace Messenger.Domain.ErrorMessages;

public static class UserErrorMessage
{
    public const string NotExistUser = "User does not exists";
    public const string UserAlreadyVerified = "User already verified";
    public const string HasActualVerifyToken = "User already has actual verification token";
    public const string DontHasActualVerifyToken = "User don't have a verification token";
    public const string ExpiredVerifyToken = "Token is expired";
}