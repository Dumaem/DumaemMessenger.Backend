namespace Messenger.Domain.Exception;

public class ChatException : System.Exception
{
    public ChatException(string? message) : base(message)
    {
    }

    public ChatException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}