using Microsoft.Extensions.Configuration;

namespace TechECommerceServer.Persistence.Configurations
{
    internal class DefaultDbConnectionStringConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/TechECommerceServer.API"));
                configurationManager.AddJsonFile("appsettings.Development.json");

                return configurationManager.GetConnectionString("Default");
            }
        }
    }
}
