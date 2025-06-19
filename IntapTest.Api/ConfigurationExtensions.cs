using IntapTest.Shared.AppConfigurations;
using Microsoft.Identity.Client;

namespace IntapTest.Api
{
    public static class ConfigurationExtensions
    {
        public static AppConfiguration GetApplicationConfig(this IConfiguration configuration)
        {
            var config = new AppConfiguration();
            configuration.GetSection("AppConfiguration").Bind(config);
            return config;
        }
    }
}
