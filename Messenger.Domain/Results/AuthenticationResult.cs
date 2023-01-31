using Messenger.Domain.Models;

namespace Messenger.Domain.Results;

public class AuthenticationResult : BaseResult
{
    public JwtToken? Token { get; init; }
}