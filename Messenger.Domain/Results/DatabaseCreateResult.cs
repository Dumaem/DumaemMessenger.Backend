using System.Numerics;
using Messenger.Domain.Results;

namespace Messenger.Database.Write;

public class DatabaseCreateResult : BaseResult
{
    public long ObjectId { get; init; }
}