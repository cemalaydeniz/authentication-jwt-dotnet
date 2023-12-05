namespace authentication_jwt_dotnet.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Foreign keys
        public User user { get; set; } = null!;
    }
}
