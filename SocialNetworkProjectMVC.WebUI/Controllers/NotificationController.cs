using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    // private readonly fields for injecting : 
    private readonly INotificationService _notificationService;
    private readonly UserManager<CustomIdentityUser> _userManager;

    // parametric constructor for injecting :
    public NotificationController(INotificationService notificationService, UserManager<CustomIdentityUser> userManager)
    {
        _notificationService = notificationService;
        _userManager = userManager;
    }

    [HttpGet("AddNotification")]
    public async Task<IActionResult> AddNotification(string senderId, string receiverId, string notificationText)
    {
        await _notificationService.AddNotificationAsync(senderId,receiverId,notificationText);
        return Ok();
    }

    [HttpGet("RemoveNotification")]
    public async Task<IActionResult> RemoveNotification(int notificationId)
    {
        await _notificationService.RemoveNotificationAsync(notificationId);
        return Ok();
    }

    [HttpGet("RemoveAllNotificationsOfUser")]
    public async Task<IActionResult> RemoveAllNotificationsOfUser()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return BadRequest();
        await _notificationService.RemovingAllNotificationsOfUserAsync(user.Id);
        return NoContent();
    }
}
