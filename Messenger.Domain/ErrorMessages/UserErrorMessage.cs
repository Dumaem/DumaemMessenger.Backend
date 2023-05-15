namespace Messenger.Domain.ErrorMessages;

public static class UserErrorMessage
{
    public const string NotExistUser = "Такого пользователя не существует";
    public const string UserAlreadyVerified = "Пользователь уже верифицирован";
    public const string HasActualVerifyToken = "У пользователя уже существует код для верификации";
    public const string DontHasActualVerifyToken = "Для данного пользователя не существует кода верификации";
    public const string ExpiredVerifyToken = "Код верификации истек";
}