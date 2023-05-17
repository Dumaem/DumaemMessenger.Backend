using Messenger.Domain.Services;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

public class ChatController : AuthorizedControllerBase
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
        var userId = ParseHttpClaims().Id;
        if (!credentials.ParticipantsIds.Contains(userId))
            return BadRequest("Cannot create a chat without self");
        var result = await _chatService.CreateChatAsync(credentials.ParticipantsIds, credentials.GroupName);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok();
    }

    [HttpGet]
    [Route("get-chat-by-name")]
    public async Task<IActionResult> GetChat([FromQuery] string name)
    {
        var result = await _chatService.GetChatAsync(name, ParseHttpClaims().Id);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Entity);
    }

    [HttpGet]
    [Route("get-chat-by-id")]
    public async Task<IActionResult> GetChat([FromQuery] int id)
    {
        var result = await _chatService.GetChatAsync(id, ParseHttpClaims().Id);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Entity);
    }

    [HttpGet]
    [Route("get-chat-members-by-name")]
    public async Task<IActionResult> GetChatMembers([FromQuery] string name)
    {
        var result = await _chatService.GetChatParticipantsAsync(name);
        return Ok(result);
    }

    [HttpGet]
    [Route("get-chat-members-by-id")]
    public async Task<IActionResult> GetChatMembers([FromQuery] int id)
    {
        var result = await _chatService.GetChatParticipantsAsync(id);
        return Ok(result);
    }

    [HttpGet]
    [Route("get-user-chats-by-email")]
    public async Task<IActionResult> GetUserChats([FromQuery] string email)
    {
        var result = await _chatService.GetChatsForUserAsync(email);
        return Ok(result);
    }

    [HttpGet]
    [Route("get-user-chats-by-id")]
    public async Task<IActionResult> GetUserChats([FromQuery] int id)
    {
        var result = await _chatService.GetChatsForUserAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [Route("add-member-to-chat")]
    public async Task<IActionResult> AddMemberToChat([FromQuery] string chatId, [FromQuery] IEnumerable<int> userIds)
    {
        if (!await _chatService.IsMemberParted(chatId, ParseHttpClaims().Id))
            return BadRequest("You are not parted in this chat to add members");
        var result = await _chatService.AddMembersToChatAsync(chatId, userIds);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }
}