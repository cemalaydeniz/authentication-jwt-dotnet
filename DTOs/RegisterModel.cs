namespace authentication_jwt_dotnet.DTOs
{
    public class RegisterModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Name { get; set; }
    }
}
