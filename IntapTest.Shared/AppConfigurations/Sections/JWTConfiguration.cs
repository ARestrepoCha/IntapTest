namespace IntapTest.Shared.AppConfigurations.Sections
{
    public class JWTConfiguration
    {
        public string TokenSecretKey { get; set; }
        public string ExpiryInMinutes { get; set; }
        public string AccessRefreshToken { get; set; }
        public string AccessRefreshTokenExpirationMinutes { get; set; }
    }
}
