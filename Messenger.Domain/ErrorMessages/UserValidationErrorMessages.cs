namespace Messenger.Domain.ErrorMessages;

public static class UserValidationErrorMessages
{
    public const string EmptyEmail = "Не указана электронная почта";
    public const string IncorrectEmail = "Неверно указана почта";
 
    public const string EmptyName = "Не указано имя";
    public const string TooShortName = "Слишком короткое имя";
    public const string TooLongName = "Слишком длинное имя";
    public const string WrongName = "Имя содержит недопустимые символы";

    public const string TooLongUsername = "Слишком длинное имя пользователя";
    public const string TooShortUsername = "Слишком короткое имя пользователя";
    public const string WrongUsername = "Имя пользователя содержит недопустимые символы";

    public const string EmptyPassword = "Пароль не может быть пустым";
    public const string TooShortPassword = "Слишком короткий пароль";
    public const string TooLongPassword = "Слишком длинный пароль";
}