using System.Threading.Tasks;
using Identity.API.Models;

namespace Identity.API.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> Login(User userModel);

        Task<AuthenticationResponse> Register(User userModel);
    }
}