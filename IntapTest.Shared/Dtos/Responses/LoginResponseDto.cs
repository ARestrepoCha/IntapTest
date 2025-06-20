namespace IntapTest.Shared.Dtos.Responses
{
    public class LoginResponseDto
    {
        public string? ExpirationTime { get; set; }
        public string? TokenValue { get; set; }
        public string? ExpirationRefreshTime { get; set; }
        public string? RefreshTokenValue { get; set; }
    }
}
