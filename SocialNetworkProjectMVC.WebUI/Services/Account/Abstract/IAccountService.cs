using SocialNetworkProjectMVC.WebUI.Models.Account;

namespace SocialNetworkProjectMVC.WebUI.Services.Account.Abstract;
public interface IAccountService
{
    // methods that will be implemented in other classes : 
    public Task<bool> RegisterAsync(RegisterViewModel registerViewModel);
    public Task<bool> LoginAsync(LoginViewModel loginViewModel);
    public Task<bool> LogOutAsync();
}
