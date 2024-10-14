namespace SocialNetworkProjectMVC.WebUI.Services.Other.Abstract;
public interface IImageService
{
    // public methods that will be implemented in other classes: 
    public Task<string> SaveFileAsync(IFormFile file);
}
