using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Enums;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Business.Concrete;
public class FriendRequestService : IFriendRequestService
{
    // private readonly fields for injecting :
    private readonly IFriendRequestDal _friendRequestDal;
    private readonly IHttpContextAccessor _context;
    private readonly IFriendService _friendService;
    private readonly UserManager<CustomIdentityUser> _userManager;

    // parametric constructor for injecting : 
    public FriendRequestService(IFriendRequestDal friendRequestDal, IHttpContextAccessor context, UserManager<CustomIdentityUser> userManager, IFriendService friendService)
    {
        _friendRequestDal = friendRequestDal;
        _context = context;
        _userManager = userManager;
        _friendService = friendService;
    }
    public async Task AcceptFriendRequestAsync(string senderId)
    {
        var friendRequests = await _friendRequestDal.GetListAsync();

        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);

        var updatedRequest =  friendRequests.FirstOrDefault(fr => fr.SenderId == senderId && fr.ReceiverId == currentUser.Id);
        updatedRequest.Status = RequestStatus.Accepted;
        updatedRequest.AcceptedDate = DateTime.Now;
        await _friendRequestDal.UpdateAsync(updatedRequest);

        var newRequest = new FriendRequest
        {
            SenderId = currentUser.Id,
            ReceiverId = senderId,
            Status = RequestStatus.Accepted,
            AcceptedDate = DateTime.Now,
        };
        await _friendRequestDal.AddAsync(newRequest);

        await _friendService.AddFriendAsync(senderId);
    }

    public async Task CancelFriendRequestAsync(string receiverId)
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var friendRequests = await _friendRequestDal.GetListAsync();
        var friendRequest = friendRequests.FirstOrDefault(fr => fr.SenderId == currentUser.Id && fr.ReceiverId == receiverId);
        await _friendRequestDal.DeleteAsync(friendRequest);
    }

    public async Task<List<FriendRequest>> GetAllFriendRequestsAsync()
    {
        var friendRequests = await _friendRequestDal.GetListAsync();
        return friendRequests;
    }

    public async Task<List<FriendRequest>> GetAllSentFriendRequestsOfCurrentUserAsync()
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var friendRequests = await _friendRequestDal.GetListAsync();
        return friendRequests.Where(fr => fr.ReceiverId == currentUser.Id && fr.Status == RequestStatus.Pending).ToList();
    }

    public async Task<List<FriendRequest>> GetFriendRequestsCurrentUserAsync(string key = "")
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var friendRequests = await _friendRequestDal.GetListAsync();
        if(key != "") return friendRequests.Where(fr => fr.ReceiverId == currentUser.Id || fr.SenderId == currentUser.Id && fr.Receiver.UserName.Contains(key) || fr.Sender.UserName.Contains(key)).ToList();
        return friendRequests.Where(fr => fr.ReceiverId == currentUser.Id && fr.Status == RequestStatus.Pending || fr.SenderId == currentUser.Id && fr.Status == RequestStatus.Pending).ToList();
    }

    public async Task RejectFriendRequestAsync(string senderId)
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var friendRequests = await _friendRequestDal.GetListAsync();
        var friendRequest = friendRequests.FirstOrDefault(fr => fr.SenderId == senderId && fr.ReceiverId == currentUser.Id);
        await _friendRequestDal.DeleteAsync(friendRequest);
    }

    public async Task SendFriendRequestAsync(string receiverId)
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var newFriendRequest = new FriendRequest
        {
            SenderId = currentUser.Id,
            ReceiverId = receiverId,
            RequestDate = DateTime.Now,
            Status = RequestStatus.Pending,
        };
        await _friendRequestDal.AddAsync(newFriendRequest);
    }
}
