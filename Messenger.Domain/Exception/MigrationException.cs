namespace Messenger.Domain.Exception;

public class MigrationException : System.Exception
{
    public MigrationException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}