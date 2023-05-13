using Messenger.Domain.Models;

namespace Messenger.WebAPI.Credentials;

public class ChatCreateCredentials
{
    public IEnumerable<int> ParticipantsIds { get; set; } = null!;
    public bool IsPersonal { get; set; }
    public string? GroupName { get; set; }
}