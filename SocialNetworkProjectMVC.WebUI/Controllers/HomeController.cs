using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkProjectMVC.Entities.Models;
using SocialNetworkProjectMVC.WebUI.Consts;
using SocialNetworkProjectMVC.WebUI.Models;
using System.Diagnostics;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
public class HomeController : Controller
{
    private readonly UserManager<CustomIdentityUser> _userManager;
    public HomeController(UserManager<CustomIdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    // notifications of current user :
    public IActionResult Notifications()=> View();

    // friends of current user : 
    public IActionResult Friends() => View();

    // messages of current user : 
    public IActionResult Messages() => View();

    // index page of user :
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        if (currentUser != null) return View();
        return RedirectToAction(WebUIConstants.LoginConstant, WebUIConstants.AccountConstant);
    }
}
