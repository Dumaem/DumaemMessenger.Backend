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
        var res = await _messageService.ListMessagesAsync(chatName, ParseHttpClaims().Id, count, page * count);
        return Ok(res);
    }
    
    [HttpGet]
    [Route("getFromCount")]
    public async Task<IActionResult> ListFromInitCount([FromQuery] string chatName,[FromQuery] int initialCount, 
        [FromQuery] int count = 50, [FromQuery] int page = 0)
    {
        var res = await _messageService.ListMessagesAsync(chatName, initialCount, ParseHttpClaims().Id, count, page * count);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMessageForUser([FromQuery] long messageId)
    {
        await _messageService.DeleteMessageAsync(messageId, ParseHttpClaims().Id);
        return Ok();
    }
}