using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SocialNetworkProjectMVC.Entities.Data;
public class ZustDBContextFactory : IDesignTimeDbContextFactory<ZustDBContext>
{
    public ZustDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ZustDBContext>();

        // build configuration to access the connection string from appsettings.json :
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Make sure this points to the correct folder where appsettings.json is
            .AddJsonFile("appsettings.json")
            .Build();

        // get the connection string from the configuration :
        var connectionString = configuration.GetConnectionString("Default");

        // configure the DbContext with the connection string :
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.UseLazyLoadingProxies();

        return new ZustDBContext(optionsBuilder.Options);
    }
}
