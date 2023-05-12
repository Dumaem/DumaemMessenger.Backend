﻿using Messenger.Domain.Results;
using Messenger.Domain.Services;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
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
            result = await _chatService.CreateChatAsync(credentials.ParticipantsIds, credentials.GroupName!,
                credentials.CurrentUserId);
        }
        else
        {
            result = await _chatService.CreatePersonalChatAsync(credentials.ParticipantsIds.Last(),
                credentials.CurrentUserId);
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
        return Ok(result.Entity);
    }

    [HttpGet]
    [Route("get-chat-by-id")]
    public async Task<IActionResult> GetChatById([FromQuery] int id)
    {
        var result = await _chatService.GetChatByIdAsync(id);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Entity);
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
    public async Task<IActionResult> GetUserChats([FromQuery] string email)
    {
        var result = await _chatService.GetChatsForUserAsync(email);
        if(!result.Any())
            return BadRequest();
        return Ok(result);
    }
    
    [HttpPost]
    [Route("add-member-to-chat")]
    public async Task<IActionResult> AddMemberToChat([FromQuery] int chatId, [FromQuery] int userId)
    {
        var result = await _chatService.AddMemberToChatAsync(chatId, userId);
        if(!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }
}