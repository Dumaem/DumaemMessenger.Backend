namespace Messenger.Domain.Results;

public class ListDataResult<T> : BaseResult
{
    public required IEnumerable<T> Items { get; init; }
    public int TotalItemsCount { get; init; }
}