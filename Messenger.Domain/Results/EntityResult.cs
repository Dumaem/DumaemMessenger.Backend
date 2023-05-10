namespace Messenger.Domain.Results;

public class EntityResult<T> : BaseResult
{
    public T Entity { get; set; } = default!;
}