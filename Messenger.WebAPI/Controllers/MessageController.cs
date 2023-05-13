using Messenger.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

[ApiController]
// [Authorize]
[Route("/api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string chatName, [FromQuery] int count = 50, [FromQuery] int page = 0)
    {
        var res = await _messageService.ListMessagesAsync(chatName, count, page * count);
        return Ok(res);
    }
}