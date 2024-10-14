using Microsoft.AspNetCore.Mvc;
using SocialNetworkProjectMVC.Business.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    // private readonly fields for injecting : 
    private readonly IFriendService _friendService;

    // parametric constructor for injecting :
    public FriendController(IFriendService friendService)
    {
        _friendService = friendService;
    }

    // removing friend : 
    [HttpGet("RemoveFriend")]
    public async Task<IActionResult> RemoveFriend(string friendId)
    {
        await _friendService.RemoveFriendAsync(friendId);
        return Ok();
    }

    // getting all friends of current user : 
    [HttpGet("GetAllFriendsOfCurrentUser")]
    public async Task<IActionResult> GetAllFriendsOfCurrentUser(string key = "")
    {
        var allFriends =await _friendService.GetFriendsOfCurrentUserAsync(key);
        return Ok(new { AllFriends = allFriends});
    }

    // getting all other users : 
    [HttpGet("GetOtherPeople")]
    public async Task<IActionResult> GetOtherPeople(string key = "")
    {
        var otherPeople = await _friendService.GetOtherPeopleAsync(key);
        return Ok(new {OtherPeople = otherPeople});
    }
}
