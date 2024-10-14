using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkProjectMVC.Entities.Models;
using SocialNetworkProjectMVC.WebUI.Consts;
using SocialNetworkProjectMVC.WebUI.Models.Account;
using SocialNetworkProjectMVC.WebUI.Services.Account.Abstract;
using SocialNetworkProjectMVC.WebUI.Services.Other.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Services.Account.Concrete;
// service class for helping registering and logging user in :
public class AccountService : IAccountService
{
    // private read-only fields for injecting : 
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly RoleManager<CustomIdentityRole> _roleManager;
    private readonly SignInManager<CustomIdentityUser> _signInManager;
    private readonly IImageService _imageService;
    private readonly IHttpContextAccessor _contextAccessor;

    // parametric constructor for injectings : 
    public AccountService(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, IImageService imageService, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _imageService = imageService;
        _contextAccessor = contextAccessor;
    }

    // implemented methods from 'IAccountService' interface :
    public async Task<bool> LoginAsync(LoginViewModel loginViewModel)
    {
        SignInResult result = await _signInManager.PasswordSignInAsync(loginViewModel.Username,loginViewModel.Password,loginViewModel.RememberMe,false);
        if (result.Succeeded)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == loginViewModel.Username);
            user.IsOnline = true;
            await _userManager.UpdateAsync(user);
            return true;
        }
        return false;
    }

    public async Task<bool> RegisterAsync(RegisterViewModel registerViewModel)
    {
        if(registerViewModel.ProfileImageFile != null) await _imageService.SaveFileAsync(registerViewModel.ProfileImageFile);
        if(registerViewModel.BgImageFile != null) await _imageService.SaveFileAsync(registerViewModel.BgImageFile);
        var user = new CustomIdentityUser
        {
            Email = registerViewModel.Email,
            UserName = registerViewModel.Username,
            ImageUrl = registerViewModel.ImageUrl,
            BackgroundImageUrl = registerViewModel.BgImageUrl,
        };
        IdentityResult result = await _userManager.CreateAsync(user,registerViewModel.Password);
        if (result.Succeeded)
        {
            if(!await _roleManager.RoleExistsAsync(WebUIConstants.MemberRole))
            {
                CustomIdentityRole newRole = new CustomIdentityRole { Name = WebUIConstants.MemberRole };
                IdentityResult roleResult = await _roleManager.CreateAsync(newRole);
            }
        }
        await _userManager.AddToRoleAsync(user, WebUIConstants.MemberRole);
        return true;
    }

    public async Task<bool> LogOutAsync()
    {
        var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
        if (user != null)
        {
            user.IsOnline = false;
            user.DisconnectTime = DateTime.UtcNow;
            await _signInManager.SignOutAsync();
            await _userManager.UpdateAsync(user);
            _contextAccessor.HttpContext.Session.SetString("UserViewModel", "");
            return true;
        }
        return false;
    }
}
