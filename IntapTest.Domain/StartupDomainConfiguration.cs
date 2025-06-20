using IntapTest.Domain.Services.Users;
using IntapTest.Shared.AppConfigurations;
using IntapTest.Shared.AppConfigurations.Sections;
using Microsoft.Extensions.DependencyInjection;

namespace IntapTest.Domain
{
    public class StartupDomainConfiguration
    {
        public void DomainConfigureServices(IServiceCollection services, AppConfiguration appConfiguration)
        {
            services.AddTransient<IUserService, UserService>().Configure<JWTConfiguration>(x =>
            {
                x.ExpiryInMinutes = appConfiguration.JWTConfiguration.ExpiryInMinutes;
                x.AccessRefreshTokenExpirationMinutes = appConfiguration.JWTConfiguration.AccessRefreshTokenExpirationMinutes;
                x.AccessRefreshToken = appConfiguration.JWTConfiguration.AccessRefreshToken;
                x.TokenSecretKey = appConfiguration.JWTConfiguration.TokenSecretKey;
            });
        }
    }
}
