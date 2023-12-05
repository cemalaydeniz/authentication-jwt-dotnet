using System.ComponentModel.DataAnnotations;

namespace authentication_jwt_dotnet.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = null!;
        public string PasswordHashed { get; set; } = null!;
        public string? Name { get; set; }

        // Foreign keys
        public virtual ICollection<Role> Roles { get; set; } = null!;
    }
}
