namespace Messenger.Domain.Exception;

public class ChatException : System.Exception
{
    public ChatExceptionType ExceptionType { get; }
    public ChatException(ChatExceptionType type, string? message) : base(message)
    {
        ExceptionType = type;
    }

    public ChatException(ChatExceptionType type,string? message, System.Exception? innerException) : base(message, innerException)
    {
        ExceptionType = type;
    }
}