using IntapTest.Shared.AppConfigurations.Sections;

namespace IntapTest.Shared.AppConfigurations
{
    public class AppConfiguration : IAppConfiguration
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public JWTConfiguration JWTConfiguration { get; set; }
    }
}
