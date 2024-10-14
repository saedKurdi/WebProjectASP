using Microsoft.AspNetCore.Mvc;
using SocialNetworkProjectMVC.WebUI.Consts;
using SocialNetworkProjectMVC.WebUI.Services.Account.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
public class SettingsController : Controller
{
    // private readonly fields for injecting : 
    private readonly IAccountService _accountService;

    // injecting account service : 
    public SettingsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // current user's profile :
    public IActionResult MyProfile()
    {
        return View();
    }

    // opening setting of account :
    public IActionResult Setting()
    {
        return View();
    }
    
    // privacy info of web-site :
    public IActionResult Privacy()
    {
        return View();
    }

    // help & support : 
    public IActionResult HelpAndSupport()
    {
        return View();
    }

    // logout from current user :
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogOutAsync();
        return RedirectToAction(WebUIConstants.LoginConstant, WebUIConstants.AccountConstant);
    }
}
