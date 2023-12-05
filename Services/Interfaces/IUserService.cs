using authentication_jwt_dotnet.Models;
using System.Linq.Expressions;

namespace authentication_jwt_dotnet.Services.Interfaces
{
    public interface IUserService
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> FindAsync(string id);
        Task<User?> FindAsync(Expression<Func<User, bool>> query);
        Task DeleteAsync(string id);
        Task DeleteAsync(User user);
    }
}
