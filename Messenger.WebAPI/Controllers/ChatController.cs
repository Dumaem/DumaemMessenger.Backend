using Messenger.Domain.Results;
using Messenger.Domain.Services;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(ILogger<ChatController> logger, IChatService chatService)
    {
        _logger = logger;
        _chatService = chatService;
    }
    
    [HttpPost]
    [Route("create-chat")]
    public async Task<IActionResult> CreateChat([FromBody] ChatCreateCredentials credentials)
    {
        BaseResult result;
        if (!credentials.IsPersonal)
        {
            result = await _chatService.CreateChatAsync(credentials.Participants);
        }
        else
        {
            result = await _chatService.CreatePersonalChatAsync(credentials.Participants.Last());
        }
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok();
    }

    [HttpGet]
    [Route("get-chat-by-name")]
    public async Task<IActionResult> GetChatByName([FromQuery] string name)
    {
        var result = await _chatService.GetChatByNameAsync(name);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Chat);
    }
}