namespace Messenger.Domain.ErrorMessages;

public class RefreshTokenErrorMessages
{
    public const string InvalidToken = "Неверный токен";
    public const string UnrecognizedToken = "Нераспознанный токен";
    public const string UsedToken = "Данный токен уже был использован";
    public const string ExpiredToken = "Токен истек";
}