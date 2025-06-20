namespace IntapTest.Shared.Dtos.Requests
{
    public class CreateUserRequestDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirmation { get; set; }

    }
}
