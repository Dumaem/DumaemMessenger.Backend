using Messenger.Domain.ErrorMessages;

namespace Messenger.Domain.Exception;

public class ChatException : System.Exception
{
    private static readonly IReadOnlyDictionary<ChatExceptionType, string> errorMessages =
        new Dictionary<ChatExceptionType, string>
        {
            { ChatExceptionType.ExpiredToken, ServerErrorMessages.TokenIsExpired },
            { ChatExceptionType.WrongToken, ServerErrorMessages.TokenDoesNotBelongToUser },
        };

    public ChatExceptionType ExceptionType { get; }

    public ChatException(ChatExceptionType type) : base(errorMessages[type])
    {
        ExceptionType = type;
    }

    public ChatException(ChatExceptionType type, System.Exception? innerException) : base(errorMessages[type],
        innerException)
    {
        ExceptionType = type;
    }
}