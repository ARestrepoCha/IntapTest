using IntapTest.Shared.AppConfigurations.Sections;

namespace IntapTest.Shared.AppConfigurations
{
    public interface IAppConfiguration
    {
        ConnectionStrings ConnectionStrings { get; set; }
        JWTConfiguration JWTConfiguration { get; set; }
    }
}
