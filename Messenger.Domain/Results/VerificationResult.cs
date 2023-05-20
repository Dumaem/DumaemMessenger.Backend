namespace Messenger.Domain.Results;

public class VerificationResult : BaseResult
{
    public string Token { get; set; } = null!;
}