using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.Business.Concrete;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.DataAccess.Concrete.EntityFramework;
using SocialNetworkProjectMVC.Entities.Data;
using SocialNetworkProjectMVC.Entities.Models;
using SocialNetworkProjectMVC.WebUI.Hubs;
using SocialNetworkProjectMVC.WebUI.Services.Account.Abstract;
using SocialNetworkProjectMVC.WebUI.Services.Account.Concrete;
using SocialNetworkProjectMVC.WebUI.Services.Other.Abstract;
using SocialNetworkProjectMVC.WebUI.Services.Other.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional for pretty-printing JSON
        
    })
    .AddNewtonsoftJson(options =>
    {
        // Customize the settings if needed
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    }); 


// adding context with options : 
var connection = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ZustDBContext>(options =>
{
    options.UseSqlServer(connection);
    options.UseLazyLoadingProxies();
});

// adding session : 
builder.Services.AddSession();

// adding identity to project : 
builder.Services
    .AddIdentity<CustomIdentityUser, CustomIdentityRole>()
    .AddEntityFrameworkStores<ZustDBContext>()
    .AddDefaultTokenProviders();

// adding injection of http context : 
 builder.Services.AddHttpContextAccessor();

// adding injection of DAL's : 
builder.Services.AddScoped<IMessageDal, EFMessageDal>();
builder.Services.AddScoped<IChatDal,EFChatDal>();
builder.Services.AddScoped<INotificationDal, EFNotificationDal>();
builder.Services.AddScoped<IFriendRequestDal,EFFriendRequestDal>();
builder.Services.AddScoped<IFriendDal,EFFriendDal>();

// adding injections of services :
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<IFriendService, FriendService>();

// adding signalR to project : 
builder.Services.AddSignalR();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// building and starting app :
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// adding hub endpoints to project : 
app.MapHub<ZustHub>("/zustHub");

app.Run();
