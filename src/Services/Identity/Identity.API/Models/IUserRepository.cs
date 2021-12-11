using System.Threading.Tasks;
using Identity.API.Models;

namespace Identity.API.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> AnyByEmailAsync(string email);

        Task<User> GetByEmailAsync(string email);
    }
}