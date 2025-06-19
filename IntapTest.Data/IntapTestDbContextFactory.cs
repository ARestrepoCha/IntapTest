using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IntapTest.Data
{
    public class IntapTestDbContextFactory : IDesignTimeDbContextFactory<IntapTestDbContext>
    {
        public IntapTestDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../IntapTest.Api");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetSection("AppConfiguration:ConnectionStrings:DefaultConnection").Value;

            var optionsBuilder = new DbContextOptionsBuilder<IntapTestDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new IntapTestDbContext(optionsBuilder.Options);
        }
    }
}
