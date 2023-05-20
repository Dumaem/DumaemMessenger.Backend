using Messenger.Domain.Models;

namespace Messenger.Domain.Results;

public class AuthenticationResult : BaseResult
{
    public int UserId { get; set; }
    public JwtToken? Token { get; init; }
}