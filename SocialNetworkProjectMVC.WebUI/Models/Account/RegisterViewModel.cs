using System.ComponentModel.DataAnnotations;

namespace SocialNetworkProjectMVC.WebUI.Models.Account;
// creating view model class for keeping data for registering user :
public class RegisterViewModel
{
    // public properties : 
    [Required]
    public string? Username { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    [Required(ErrorMessage = "You must accept the privacy policy.")]
    public bool AcceptPrivacy { get; set; } 

    public IFormFile ? ProfileImageFile { get; set; }
    public string? ImageUrl { get; set; } = "person.png";
    public IFormFile ? BgImageFile { get; set; }
    public string? BgImageUrl { get; set; } = "personcover.jpg";

}
