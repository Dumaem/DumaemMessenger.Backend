namespace Messenger.Domain.Results
{
    public class BaseResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
    }
}
