using IntapTest.Data.Base;
using IntapTest.Data.Repositories;
using IntapTest.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IntapTest.Data
{
    public class StartupDataConfiguration
    {
        public void DataConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRequestInfo<IntapTestDbContext>, RequestInfo<IntapTestDbContext>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ITimeActivityRepository, TimeActivityRepository>();
        }
    }
}
