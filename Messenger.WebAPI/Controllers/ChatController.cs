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
            result = await _chatService.CreateChatAsync(credentials.Participants, credentials.GroupName!);
        }
        else
        {
            result = await _chatService.CreatePersonalChatAsync(credentials.Participants.Last(),
                credentials.CurrentUser!);
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

    [HttpGet]
    [Route("get-chat-members")]
    public async Task<IActionResult> GetChatMembers([FromQuery] string name)
    {
        var result = await _chatService.GetChatParticipantsAsync(name);
        if(!result.Any())
            return BadRequest();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("get-user-chats")]
    public async Task<IActionResult> GetUser([FromQuery] string email)
    {
        var result = await _chatService.GetChatsForUserAsync(email);
        if(!result.Any())
            return BadRequest();
        return Ok(result);
    }
    
    [HttpPost]
    [Route("add-member-to-chat")]
    public async Task<IActionResult> GetUser([FromBody] int chatId, int userId)
    {
        var result = await _chatService.AddMemberToChatAsync(chatId, userId);
        if(!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }
}