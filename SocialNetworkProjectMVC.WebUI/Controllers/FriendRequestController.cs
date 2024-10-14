using Microsoft.AspNetCore.Mvc;
using SocialNetworkProjectMVC.Business.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FriendRequestController : ControllerBase
{
    // private readonly fields for injecting : 
    private readonly IFriendRequestService _friendRequestService;

    // parametric constructor for injecting :
    public FriendRequestController(IFriendRequestService friendRequestService)
    {
        _friendRequestService = friendRequestService;
    }

    // sending friend request :
    [HttpGet("SendFriendRequest")]
    public async Task<IActionResult> SendFriendRequest(string receiverId)
    {
        await _friendRequestService.SendFriendRequestAsync(receiverId);
        return Ok();
    }

    // accepting friend request : 
    [HttpGet("AcceptFriendRequest")]
    public async Task<IActionResult> AcceptFriendRequest(string senderId)
    {
        await _friendRequestService.AcceptFriendRequestAsync(senderId);
        return Ok();
    }

    // rejecting - removing friend request :
    [HttpGet("RejectFriendRequest")]
    public async Task<IActionResult> RejectFriendRequest(string senderId)
    {
        await _friendRequestService.RejectFriendRequestAsync(senderId);
        return Ok();
    }

    // cancel friend request : 
    [HttpGet("CancelFriendRequest")]
    public async Task<IActionResult> CancelFriendRequest(string receiverId)
    {
        await _friendRequestService.CancelFriendRequestAsync(receiverId);
        return Ok();
    }

    // getting all friend requests of user :
    [HttpGet("GetAllFriendRequestsOfCurrentUser")]
    public async Task<IActionResult> GetAllFriendRequestsOfCurrentUser(string key = "")
    {
        var friendRequests = await _friendRequestService.GetFriendRequestsCurrentUserAsync(key);
        return Ok(new { FriendRequests = friendRequests });
    }

    // getting all friend requests from db : 
    [HttpGet("GetAllFriendRequests")]
    public async Task<IActionResult> GetAllFriendRequests()
    {
        var allFriendRequests = await _friendRequestService.GetAllFriendRequestsAsync();
        return Ok(new {AllFriendRequests = allFriendRequests});
    }

    // getting all sent requests of user :
    [HttpGet("GetAllSentFriendRequestsOfCurrentUser")]
    public async Task<IActionResult>GetAllSentFriendRequestsOfCurrentUserAsync()
    {
        var sentFriendRequests = await _friendRequestService.GetAllSentFriendRequestsOfCurrentUserAsync();
        return Ok(new {SentFriendRequests = sentFriendRequests});
    }
}
