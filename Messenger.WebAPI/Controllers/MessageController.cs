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
    [Route("/list")]
    public IActionResult List([FromQuery] int count = 50, [FromQuery] int page = 0)
    {
        var res = _messageService.ListMessagesAsync("testChat1", 10, 0);
        return Ok();
    }
}