using System.Linq;
using System.Threading.Tasks;
using Identity.API.Models;
using Identity.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public UserRepository(IdentityDbContext context)
            : base(context)
        {
        }

        public async Task<bool> AnyByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(p => p.Email == email);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.Where(p => p.Email == email).SingleOrDefaultAsync();
        }
    }
}