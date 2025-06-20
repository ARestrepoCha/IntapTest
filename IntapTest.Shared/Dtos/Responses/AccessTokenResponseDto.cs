namespace IntapTest.Shared.Dtos.Responses
{
    public class AccessTokenResponseDto
    {
        public string? Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
