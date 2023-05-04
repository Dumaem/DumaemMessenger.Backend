using System.Net;

namespace Messenger.WebAPI.Shared.SharedModels;

public class UnauthorizedAccessContext
{
    public required HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
}