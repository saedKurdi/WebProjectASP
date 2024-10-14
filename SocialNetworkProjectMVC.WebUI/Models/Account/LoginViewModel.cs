using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProjectMVC.WebUI.Models.Account;
// creating view model class for keeping data for logging user in :
public class LoginViewModel
{
    // public properties : 
    [Required]
    public string? Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}
