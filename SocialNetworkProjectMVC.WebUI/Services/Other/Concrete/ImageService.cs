using SocialNetworkProjectMVC.WebUI.Services.Other.Abstract;

namespace SocialNetworkProjectMVC.WebUI.Services.Other.Concrete;
public class ImageService : IImageService
{
    // private readonly fields for injecting : 
    private readonly IWebHostEnvironment _environment;

    // parametric constructor for injecting :
    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    // implemented methods from 'IImageService' interface : 
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var saveImg = Path.Combine(_environment.WebRootPath,"images",file.FileName);
        using (var image = new FileStream(saveImg,FileMode.OpenOrCreate))
        {
            await file.CopyToAsync(image);
        };
        return file.FileName.ToString();
    }
}
