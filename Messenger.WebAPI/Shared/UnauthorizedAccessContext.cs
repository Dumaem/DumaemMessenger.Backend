using System.Net;

namespace Messenger.WebAPI.Shared;

public class UnauthorizedAccessContext
{
    public required HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
}