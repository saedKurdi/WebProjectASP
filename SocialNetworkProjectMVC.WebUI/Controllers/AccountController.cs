using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using SocialNetworkProjectMVC.Entities.Models;
using SocialNetworkProjectMVC.WebUI.Consts;
using SocialNetworkProjectMVC.WebUI.Models.Account;
using SocialNetworkProjectMVC.WebUI.Models.Home;
using SocialNetworkProjectMVC.WebUI.Services.Account.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Controllers;
public class AccountController : Controller
{
    // private readonly fields for injecting : 
    private readonly IAccountService _accountService;
    private readonly UserManager<CustomIdentityUser> _userManager;

    // parametric constructor for injecting : 
    public AccountController(IAccountService accountService, UserManager<CustomIdentityUser> userManager)
    {
        _accountService = accountService;
        _userManager = userManager;
    }

    // for registering the user :
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords must match");
                return View(model);
            }
            if (!model.AcceptPrivacy)
            {
                ModelState.AddModelError("", "You should accept our privacy and policy");
                return View();
            }
            if(await _accountService.RegisterAsync(model))
            {
                return RedirectToAction(WebUIConstants.LoginConstant,WebUIConstants.AccountConstant);
            }
        }
        ModelState.AddModelError("", "Can not register the user !");
        return View(model);
    }

    // for logging user in : 
    [HttpGet]
    public IActionResult Login()
    { 
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
           if(await _accountService.LoginAsync(model))
            {
                return RedirectToAction(WebUIConstants.IndexConstant,WebUIConstants.HomeConstant);
            }
        }
        ModelState.AddModelError("", "Invalid Login !");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CloseAccount([FromBody] CloseAccountViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(currentUser, model.Password);

        if (isPasswordCorrect && model.Email == currentUser.Email)
        {
            var result = await _userManager.DeleteAsync(currentUser);
            if (result.Succeeded)
            {
                return RedirectToAction(WebUIConstants.LoginConstant, WebUIConstants.AccountConstant);
            }
            else
            {
                return BadRequest("Failed to delete account.");
            }
        }
        return BadRequest("Invalid email or password.");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(currentUser, model.OldPassword);

        if (isPasswordCorrect)
        {
            var result = await _userManager.ChangePasswordAsync(currentUser,model.OldPassword,model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction(WebUIConstants.LoginConstant, WebUIConstants.AccountConstant);
            }
            else
            {
                return BadRequest("Failed to change password.");
            }
        }
        return BadRequest("Invalid password.");
    }

    [HttpPost]
    public async Task<IActionResult> EditUser([FromBody] EditUserViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        if(model.Email.EndsWith("@gmail.com") && model.Email.Length>5 && model.Username.Length > 4)
        {
            currentUser.UserName = model.Username;
            currentUser.Email = model.Email;
            await _userManager.UpdateAsync(currentUser);
            return RedirectToAction(WebUIConstants.IndexConstant,WebUIConstants.HomeConstant);
        }
        return BadRequest("can not edit the user cause of invalid username and email !");
    }

    // getting current user as anonym object and returning it :
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return BadRequest();
        var obj = new {User = user};
        return Ok(obj);
    }

    // getting all users of db and returning expect current user : 
    [HttpGet]
    public async Task<IActionResult> GetAllUsers(string ? key)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return BadRequest();
        dynamic ? allUsersExpectCurrent;
        allUsersExpectCurrent = string.IsNullOrEmpty(key)  ? await _userManager.Users.Where(u => u.Id != user.Id).OrderByDescending(u => u.IsOnline).ToListAsync() : await _userManager.Users.Where(u => u.Id != user.Id && u.UserName.Contains(key)).OrderByDescending(u => u.IsOnline).ToListAsync();
        return Ok(new{ AllUsers = allUsersExpectCurrent});
    }

    [HttpGet]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u =>u.UserName == username);
        if (user == null) return BadRequest();
        return Ok(new { User = user });
    }

    [HttpGet]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return BadRequest();
        return Ok(new { User = user });
    }
}
