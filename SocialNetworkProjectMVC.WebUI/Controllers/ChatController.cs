using Microsoft.AspNetCore.Mvc;
using SocialNetworkProjectMVC.Business.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    // private readonly fields for injecting : 
    private readonly IChatService _chatService;

    // parametric constructor for injecting :
    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    // getting chats messages from db : 
    [HttpGet("GetChatMessages")]
    public async Task<IActionResult> GetChatMessages(string user1Id,string user2Id)
    {
        var chat = await _chatService.GetChatAsync(user1Id, user2Id);
        return Ok(new { Messages = chat.Messages});
    }

    // getting chat : 
    [HttpGet("GetChatBySenderReceiverId")]
    public async Task<IActionResult> GetChatBySenderReceiverId(string user1Id,string user2Id)
    {
        var chat = await _chatService.GetChatAsync(user1Id,user2Id);
        return Ok(new {Chat = chat});
    }

    // getting chat by sender or receiver : 
    [HttpGet("GetChatsBySenderOrReceiver")]
    public async Task<IActionResult> GetChatsBySenderOrReceiver(string id)
    {
        var chats = await _chatService.GetChatsByReceiverOrSenderIdAsync(id);
        return Ok(new { Chats = chats });
    }

    // deleting chat : 
    [HttpGet("ClearChatMessages")]
    public async Task<IActionResult> ClearChatMessages(string user1Id,string user2Id)
    {
        var chat = await _chatService.GetChatAsync(user1Id, user2Id);
        await _chatService.DeleteChatAsync(user1Id, user2Id);
        return Ok();
    }
}
