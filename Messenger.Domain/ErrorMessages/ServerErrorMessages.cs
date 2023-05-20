namespace Messenger.Domain.ErrorMessages;

public static class ServerErrorMessages
{
    public const string InternalServerError = "Ошибка сервера. Обратитесь в поддержку за помощью.";
    public const string TokenDoesNotBelongToUser = "Токен авторизации содержит несуществующую почту";
    public const string TokenIsExpired = "Истек срок действия токена доступа";
}