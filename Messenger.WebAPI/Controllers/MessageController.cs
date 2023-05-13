using Messenger.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

public class MessageController : AuthorizedControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string chatName, [FromQuery] int count = 50,
        [FromQuery] int page = 0)
    {
        var res = _messageService.ListMessagesAsync(chatName, ParseHttpClaims().Id, count, page * count);
        return Ok(res);
    }
}