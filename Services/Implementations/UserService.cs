using authentication_jwt_dotnet.Data;
using authentication_jwt_dotnet.Models;
using authentication_jwt_dotnet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace authentication_jwt_dotnet.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> FindAsync(string id)
        {
            return await _dbContext.Users.Include(_ => _.Roles).FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<User?> FindAsync(Expression<Func<User, bool>> query)
        {
            return await _dbContext.Users.Include(_ => _.Roles).SingleOrDefaultAsync(query);
        }

        public async Task DeleteAsync(string id)
        {
            User? user = await FindAsync(id);
            if (user == null)
                throw new Exception("User not found");      // This should throw a custom exception
                                                            // For the simplicity of the project, it will stay

            await DeleteAsync(user);
        }

        public async Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
