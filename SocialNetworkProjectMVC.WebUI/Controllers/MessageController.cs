using Microsoft.AspNetCore.Mvc;
using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.DataAccess.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    // private readonly fields for injecting : 
    private readonly IMessageService _messageService;
    private readonly IMessageDal _messageDal;

    // parametric constructor for injecting :
    public MessageController(IMessageService messageService, IMessageDal messageDal)
    {
        _messageService = messageService;
        _messageDal = messageDal;
    }

    [HttpGet("AddMessage")]
    public async Task<IActionResult> AddMessage(int chatId,string senderId, string receiverId,string messageText)
    {
        await _messageService.AddMessageAsync(chatId, senderId, receiverId, messageText);
        return Ok();
    }

    // setting messages readen of current user : 
    [HttpGet("SetMessagesReaden")]
    public async Task<IActionResult> SetMessagesReaden(string senderId, string receiverId)
    {
        var messages = await _messageDal.GetListAsync();
        foreach (var message in messages)
        {
            if (message.SenderId == senderId && message.ReceiverId == receiverId)
            {
                message.IsRead = true;
                await _messageDal.UpdateAsync(message);
            }
        }
        return Ok();
    }
}
